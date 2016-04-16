using System;

namespace Dexpa.ApiClient
{
    class Token
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public DateTime Created { get; private set; }

        public DateTime ExpiresIn
        {
            get { return Created.AddSeconds(expires_in); }
        }

        public Token()
        {
            Created = DateTime.UtcNow;
        }
    }
}
