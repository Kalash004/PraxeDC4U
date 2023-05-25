﻿using Microsoft.AspNetCore.Components;
using DataTemplateLibrary.Models;
using ServerManagement;

namespace PraxeFiverrClone.Pages
{
    public partial class ServicePage : ComponentBase
    {
        [Inject] public ServerManager? ServerManager { get; set; }
        [Parameter] public int ServiceID { get; set; }

        private DBService? Service { get; set; }

        protected override void OnInitialized()
        {
            if(ServerManager != null)
            {
                Service = ServerManager.GetService(ServiceID);
            }         
        }

        protected string ServiceName
        {
            get
            {
                return Service != null ? Service.ServiceName : ""; ;
            }
        }
        protected string ServiceDescription
        {
            get
            {
                return Service != null ? Service.ShortDescription : "";
            }
        }
        protected DateOnly ServiceCreation
        {
            get
            {
                return Service != null ? Service.Created : DateOnly.MinValue;
            }
        }
        protected int ServicePrice
        {
            get
            {
                return Service != null ? Service.CurrentPrice : 0;
            }
        }
        protected int ServiceOwner
        {
            get
            {
                return Service != null ? Service.UserId : 0;
            }
        }
    }
}