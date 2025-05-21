using ApplicantPersonalAccount.Application.OuterServices.DTO;
using ApplicantPersonalAccount.Common.Constants;
using ApplicantPersonalAccount.Infrastructure.Converters;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace ApplicantPersonalAccount.DirectoryService.Services.Implemntations
{
    public class DirectoryServiceImpl : IDirectoryService
    {
        private readonly HttpClient _httpClient;

        public DirectoryServiceImpl(HttpClient httpClient)
        {
            _httpClient = httpClient;

            var bytes = Encoding.ASCII.GetBytes($"{ServerURLs.USERNAME}:{ServerURLs.PASSWORD}");
            var credentials = Convert.ToBase64String(bytes);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);
        }

        public async Task<List<EducationLevel>> GetEducationLevels()
        {
            var response = await _httpClient.GetAsync(ServerURLs.EDUCATION_LEVELS);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<EducationLevel>>(stringResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonTimeConverter() }
            });

            return data ?? new List<EducationLevel>();
        }

        public async Task<List<DocumentType>> GetDocumentTypes()
        {
            var response = await _httpClient.GetAsync(ServerURLs.DOCUMENT_TYPES);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<DocumentType>>(stringResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonTimeConverter() }
            });

            return data ?? new List<DocumentType>();
        }

        public async Task<List<Faculty>> GetFaculties()
        {
            var response = await _httpClient.GetAsync(ServerURLs.FACULTIES);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<List<Faculty>>(stringResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonTimeConverter() }
            });

            return data ?? new List<Faculty>();
        }

        public async Task<ProgramPagedList> GetEducationPrograms(int page = 1, int size = 10)
        {
            var response = await _httpClient.GetAsync(ServerURLs.PROGRAMS + "?page=" + page + "&size=" + size);

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<ProgramPagedList>(stringResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonTimeConverter() }
            });

            return data ?? new ProgramPagedList();
        }
    }
}
