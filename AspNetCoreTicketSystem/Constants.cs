namespace AspNetCoreTicketSystem
{
    public static class Constants
    {
        public const string AdministratorRole = "Administrator";
        public const string HelpdeskRole = "Helpdesk";
        public const string CustomerRole = "Customer";
    }
}

//The Constants class in the AspNetCoreTicketSystem namespace centralizes the definition of role names used throughout the application, ensuring consistency and maintainability. This static class contains constant string fields for roles such as "Administrator," "Helpdesk," and "Customer." By using these constants, the application prevents typos and makes role management easier, as changes to role names need to be made in only one place. This approach enhances readability and reliability in various parts of the application, including authorization, role management, and access control, ensuring that role names are used consistently and correctly.