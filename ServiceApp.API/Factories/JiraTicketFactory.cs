using ServiceApp.Models;
using ServiceApp.Models.Entities;
using ServiceApp.Models.JiraEntities;

namespace ServiceApp.API.Factories
{
    public class JiraTicketFactory : IFactory
    {
        private readonly Ticket _ticket;
        private readonly IssueType _issueType;

        public JiraTicketFactory(Ticket ticket, IssueType issueType)
        {
            _ticket = ticket;
            _issueType = issueType;
        }


        public IProduct Create()
        {
            if (_ticket is not null && _issueType is not null)
            {
                return new JiraTicketRequest()
                {
                    fields = new Fields()
                    {
                        description = _ticket.Title,
                        summary = _ticket.Description,
                        issuetype = _issueType,
                        project = new Project()
                        {
                            key = "PRAC"
                        }
                    }
                };
            }
            return null;
        }
    }
}
