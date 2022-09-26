using Microsoft.AspNetCore.SignalR;
using ServiceApp.Pages.Users;

namespace ServiceApp.Tools
{
    public static class PageDictionary
    {
        public static string UserListPage() => "/list";
        public static string UserDetailsPage(string id) => $"/Userdetails/{id}";
        public static string UserEditPage(string id) => $"/edituser/{id}";
        public static string TicketsPage() => "/tickets";
        public static string TicketDetailsPage(string id) => $"/Ticketdetails/{id}";
        public static string FormPage() => $"/form";
        public static string LoginPage() => $"/login";
        public static string RegisterPage() => $"/register";
        public static string CustomersPage() => $"/customers";
        public static string IncorrectCredentialsPage(string value = null) => $"/incorrectCredentials/{value}";
        public static string SentTicketPage() => $"/sentTicketPage";
        public static string RegisteredPage() => "/registered";
        public static string IndexPage() => $"/";
        public static string MyProfilePage() => $"/myprofile";
        public static string ErrorPage() => $"/error";
    }
}
