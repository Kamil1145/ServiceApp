using Newtonsoft.Json;
using ServiceApp.API.Factories;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models.Entities;
using System.Text;
using ServiceApp.API.Exceptions;
using ServiceApp.Models.JiraEntities;
using AutoMapper;
using ServiceApp.Models.DTO;

namespace ServiceApp.API.Services
{
    public class JiraService : IJiraService
    {
        public HttpClient _httpClient { get; set; }
        public User _userContext { get; set; }
        public readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public JiraService(
            IHttpClientFactory httpClientFactory, ITicketService ticketService, IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("JiraHttpClient");
            _ticketService = ticketService;
            _mapper = mapper;
        }

        private void SetJiraCredentials()
        {
            if (_userContext.JiraCredentials is null)
                throw new JiraException("Incorrect Jira Credentials");

            string form = $"{_userContext.JiraCredentials.Username}:{_userContext.JiraCredentials.Password}";
            form = Convert.ToBase64String(Encoding.UTF8.GetBytes(form));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", form);
        }

        public async Task<TicketDto> CreateJiraTicket(Ticket ticket)
        {
            TicketDto ticketDto = null;
            var project = await GetJiraProjectIssues();
            var issue = project.issueTypes.FirstOrDefault(x => x.name == "Zadanie");
            var jiraTicket = new JiraTicketFactory(ticket, issue).Create();
            var content = new StringContent(JsonConvert.SerializeObject(jiraTicket), Encoding.UTF8, "application/json");
            var resp = await _httpClient.PostAsync("issue", content);
            
            if (resp.IsSuccessStatusCode)
            {
                var jiraTicketResponse = JsonConvert.DeserializeObject<JiraTicketResponse>(await resp.Content.ReadAsStringAsync());
                ticket.JiraTicketId = jiraTicketResponse.Key;
                ticketDto = await _ticketService.UpdateTicket(_mapper.Map<Ticket, TicketDto>(ticket));
            }

            return ticketDto;
        }

        public async Task<JiraProjectResponse> GetJiraProjectIssues()
        {
            JiraProjectResponse jiraProject = null;
            SetJiraCredentials();
            var response = await _httpClient.GetAsync("project/PRAC");
            string body = await response.Content.ReadAsStringAsync();
            try
            {
                jiraProject = JsonConvert.DeserializeObject<JiraProjectResponse>(body);
            }
            catch (Exception e)
            {
                throw new JiraException(e.Message);
            }
            return jiraProject;

        }

    }

}
