﻿using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Common.DTOs;
using ApplicantPersonalAccount.Common.DTOs.Filters;
using ApplicantPersonalAccount.Common.Enums;
using ApplicantPersonalAccount.Common.Exceptions;
using ApplicantPersonalAccount.Common.Models;
using ApplicantPersonalAccount.Common.Models.Applicant;
using ApplicantPersonalAccount.Common.Models.Enterance;
using ApplicantPersonalAccount.Common.Models.User;
using ApplicantPersonalAccount.Infrastructure.RabbitMq;
using ApplicantPersonalAccount.Infrastructure.RabbitMq.MessageProducer;
using ApplicantPersonalAccount.Notification.Models;
using ApplicantPersonalAccount.Persistence.Contextes;
using ApplicantPersonalAccount.Persistence.Entities.ApplicationDb;
using ApplicantPersonalAccount.Persistence.Entities.UsersDb;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApplicantPersonalAccount.Applicant.Services.Implementations
{
    public class EnteranceServiceImpl : IEnteranceService
    {
        private readonly ILogger<EnteranceServiceImpl> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ApplicationDataContext _applicantContext;
        private readonly IMessageProducer _messageProducer;

        public EnteranceServiceImpl(
            ILogger<EnteranceServiceImpl> logger,
            ApplicationDataContext applicationContext,
            IMessageProducer messageProducer)
        {
            _logger = logger;
            _applicantContext = applicationContext;

            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            _messageProducer = messageProducer;
        }

        public async Task<EnteranceModel> GetEnteranceByUserId(Guid userId)
        {
            var enterance = await FindEnterance(userId);

            var enteranceModel = await FormEnteranceModel(enterance);

            return enteranceModel;
        }

        private async Task<EnteranceEntity> FindEnterance(Guid id)
        {
            var enterance = await _applicantContext.Enterances
                .Include(e => e.Programs)
                .FirstOrDefaultAsync(e => e.ApplicantId == id);

            if (enterance == null)
                _logger.LogWarning($"User {id} enterance not found");

            return enterance ?? throw new NotFoundException(ErrorMessages.ENTERANCE_NOT_FOUND);
        }

        private async Task<EnteranceModel> FormEnteranceModel(EnteranceEntity enteranceEntity)
        {
            var applicantEntity = await GetUserById(enteranceEntity.ApplicantId);

            var applicantModel = new UserProfileModel
            {
                Id = applicantEntity.Id,
                Name = applicantEntity.Name,
                Role = applicantEntity.Role,
                Email = applicantEntity.Email,
                Phone = applicantEntity.Phone,
                Gender = applicantEntity.Gender,
                Birthdate = applicantEntity.Birthdate,
                Citizenship = applicantEntity.Citizenship,
                Address = applicantEntity.Address
            };

            var managerEntity = enteranceEntity.ManagerId != null ?
                await GetUserById((Guid)enteranceEntity.ManagerId) : null;

            ManagerModel? managerModel = null;

            if (managerEntity != null)
            {
                managerModel = new ManagerModel
                {
                    Name = managerEntity.Name,
                    Phone = managerEntity.Phone,
                    Email = managerEntity.Email,
                };
            }

            var enteranceProgramModels = await GetListOfEnterancePrograms(enteranceEntity);

            return new EnteranceModel
            {
                Id = enteranceEntity.Id,
                Applicant = applicantModel,
                Manager = managerModel,
                Status = enteranceEntity.Status,
                UpdateTime = enteranceEntity.UpdateTime,
                Programs = enteranceProgramModels
            };
        }

        private async Task<UserEntity> GetUserById(Guid id)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = id
            };

            var result = await rpcClient.CallAsync(request, RabbitQueues.GET_USER_BY_ID);

            rpcClient.Dispose();
            if (result == null || result == "null")
                throw new NotFoundException(ErrorMessages.USER_NOT_FOUND);

            var userData = JsonSerializer.Deserialize<UserEntity>(result, _jsonOptions)!;

            return userData;
        }

        private async Task<List<ApplicationModel>> GetListOfEnterancePrograms(EnteranceEntity enteranceEntity)
        {
            var result = new List<ApplicationModel>();

            foreach (var program in enteranceEntity.Programs)
            {
                var applicationProgramModel = await GetEducationProgramById(program.ProgramId);

                var programModel = new ApplicationModel
                {
                    Id = program.Id,
                    Priority = program.Priority,
                    Program = applicationProgramModel
                };

                result.Add(programModel);
            }

            return result;
        }

        private async Task<EducationProgramModel> GetEducationProgramById(Guid id)
        {
            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = id
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_EDUCATION_PROGRAM_BY_ID);

            rpcClient.Dispose();
            if (result == null || result == "null")
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            var educationProgram = JsonSerializer.Deserialize<EducationProgramModel>(result)!;
            
            return educationProgram;
        }

        public async Task<ApplicationModel> GetApplicationById(Guid id)
        {
            var application = await FindApplicationById(id);

            return new ApplicationModel
            {
                Id = application.Id,
                Priority = application.Priority,
                Program = await GetEducationProgramById(application.ProgramId)
            };
        }

        private async Task<EnteranceProgramEntity> FindApplicationById(Guid id)
        {
            var applicationEntity = await _applicantContext.EnterancePrograms
                .Include(p => p.Enterance)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (applicationEntity == null)
                _logger.LogWarning($"Application with id {id} not found");

            return applicationEntity ?? throw new NotFoundException(ErrorMessages.APPLICATION_NOT_FOUND);
        }

        public async Task DeleteApplicationById(Guid id)
        {
            var application = await FindApplicationById(id);

            _applicantContext.EnterancePrograms.Remove(application);
            await _applicantContext.SaveChangesAsync();

            _logger.LogInformation($"Application {id} deleted");
        }

        public async Task UpdateEnteranceStatus(Guid userId, EnteranceStatus newStatus)
        {
            var enterance = await FindEnterance(userId);

            enterance.Status = newStatus;
            await _applicantContext.SaveChangesAsync();

            _logger.LogInformation($"User {userId} enterance status now is {newStatus}");

            var user = await GetUserById(userId);

            var notification = new NotificationModel
            {
                UserEmail = user.Email,
                Title = "Enterance update",
                Text = $"Your enterance status was updated! New status: {newStatus}"
            };
            _messageProducer.SendMessage(notification, RabbitQueues.NOTIFICATION);
        }

        public async Task EditAppicationById(Guid id, 
            EducationProgramApplicationModel applicationModel,
            string actingUser, string userRole, Guid userId)
        {
            var application = await FindApplicationById(id);

            if (userRole == "Manager" &&
                (application.Enterance.ManagerId == null && application.Enterance.ManagerId != userId))
                throw new UnaccessableAction(ErrorMessages.MANAGER_CANNOT_EDIT_ENTERANCE);

            var rpcClient = new RpcClient();
            var request = new GuidRequestDTO
            {
                Id = applicationModel.ProgramId
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_EDUCATION_PROGRAM_BY_ID);
            if (result == null || result == "null")
                throw new NotFoundException(ErrorMessages.PROGRAM_IS_NOT_FOUND);

            application.Priority = applicationModel.Priority;
            application.ProgramId = applicationModel.ProgramId;

            await _applicantContext.SaveChangesAsync();

            _logger.LogInformation($"Application {id} has been updated by {actingUser}");
        }

        public async Task<EnterancePagedListModel> GetEnterances(
            string? name,
            string? program,
            List<string>? faculties,
            EnteranceStatus? status,
            bool hasManagerOnly,
            bool attachedToManager,
            SortingType? sortedByUpdateDate,
            Guid managerId,
            int page = 1,
            int size = 5)
        {
            IQueryable<EnteranceEntity> result = _applicantContext.Enterances
                .Include(e => e.Programs);

            if (name != null)
            {
                var nameIds = await FindFilteredNameList(name);
                result = result.Where(e => nameIds.Contains(e.ApplicantId));
            } 

            if (program != null || (faculties != null && faculties.Count > 0))
            {
                var programIds = await FindFilteredPrograms(program, faculties);
                result = result.Where(e => e.Programs.Any(p => programIds.Contains(p.ProgramId)));
            }

            if (status != null)
                result = result.Where(e => e.Status == status);

            if (hasManagerOnly)
                result = result.Where(e => e.ManagerId != null);

            if (attachedToManager)
                result = result.Where(e => e.ManagerId == managerId);

            if (sortedByUpdateDate == SortingType.Ascending)
                result = result.OrderBy(e => e.UpdateTime);

            if (sortedByUpdateDate == SortingType.Descending)
                result = result.OrderByDescending(e => e.UpdateTime);

            var totalCount = await result.CountAsync();

            result = result
                .Skip((page - 1) * size)
                .Take(size);

            var foundEnterances = await result.ToListAsync();

            List<EnteranceModel> enterancesModels = new List<EnteranceModel>();

            foreach (var enterance in foundEnterances)
            {
                enterancesModels.Add(await FormEnteranceModel(enterance));
            }

            return new EnterancePagedListModel
            {
                Enterances = enterancesModels,

                Pagination = new PageInfoModel
                {
                    Current = page,
                    Size = size,
                    Count = (totalCount / size) + (totalCount % size > 0 ? 1 : 0)
                }
            };
        }

        private async Task<List<Guid>> FindFilteredNameList(string name)
        {
            var rpcClient = new RpcClient();
            var request = new BrokerRequestDTO
            {
                Request = name
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_FILTERED_NAMES);
            rpcClient.Dispose();

            var list = JsonSerializer.Deserialize<ListOfIdsDTO>(result)!;
            return list.Ids;
        }

        private async Task<List<Guid>> FindFilteredPrograms(
            string? program, List<string>? faculties)
        {
            var rpcClient = new RpcClient();
            var request = new FilterByProgramDTO
            {
                Program = program == null ? String.Empty : program,
                Faculties = faculties == null ? new List<string>() : faculties
            };

            string result = await rpcClient.CallAsync(request, RabbitQueues.GET_FILTERED_PROGRAMS);
            rpcClient.Dispose();

            var list = JsonSerializer.Deserialize<ListOfIdsDTO>(result)!;
            return list.Ids;
        }

        public async Task CreateEnteranceForUser(Guid userId)
        {
            var newEnterance = new EnteranceEntity
            {
                ApplicantId = userId
            };

            _applicantContext.Enterances.Add(newEnterance);

            await _applicantContext.SaveChangesAsync();
        }
    }
}
