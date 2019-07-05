﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WQLIdentity.Application.Dtos.Clients
{
    public class ClientListDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Unique ID of the client
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Client display name (used for logging and consent screen)
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// Specifies if client is enabled (defaults to <c>true</c>)
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// URI to client logo (used on consent screen)
        /// </summary>
        public string LogoUri { get; set; }
    }
}
