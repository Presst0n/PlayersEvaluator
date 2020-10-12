namespace PE.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Users
        {
            public const string Get = Base + "/users/{userId}";
            public const string GetLoggedIn = Base + "/users/user";
            public const string GetAll = Base + "/users";
        }

        public static class Rosters
        {
            public const string Get = Base + "/rosters/{rosterId}";
            public const string GetAll = Base + "/rosters";
            public const string Create = Base + "/rosters";
            public const string Update = Base + "/rosters/{rosterId}";
            public const string Delete = Base + "/rosters/{rosterId}";
        }

        public static class Raiders
        {
            public const string Get = Base + "/raiders/{raiderId}";
            public const string GetAll = Base + "/raiders";
            public const string Create = Base + "/raiders";
            public const string Update = Base + "/raiders/{raiderId}";
            public const string Delete = Base + "/raiders/{raiderId}";
        }

        public static class RaiderNotes
        {
            public const string Get = Base + "/raiderNotes/{raiderNoteId}";
            public const string GetAll = Base + "/raiderNotes";
            public const string Create = Base + "/raiderNotes";
            public const string Update = Base + "/raiderNotes/{raiderNoteId}";
            public const string Delete = Base + "/raiderNotes/{raiderNoteId}";
        }

        public static class RosterAccesses
        {
            public const string Get = Base + "/rosterAccesses/{rosterAccessId}";
            public const string GetAll = Base + "/rosterAccesses";
            public const string Create = Base + "/rosterAccesses";
            public const string Update = Base + "/rosterAccesses/{rosterAccessId}";
            public const string Delete = Base + "/rosterAccesses/{rosterAccessId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
        }
    }
}
