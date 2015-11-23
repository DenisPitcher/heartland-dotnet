﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCheck.cs" company="Heartland Payment Systems">
//   Copyright (c) Heartland Payment Systems. All rights reserved.
// </copyright>
// <summary>
//   The check information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecureSubmit.Tests.TestData
{
    using Hps.Exchange.PosGateway.Client;
    using SecureSubmit.Entities;

    public static class TestCheck
    {
        /// <summary>Various test credit cards</summary>
        private static HpsCheck approve = new HpsCheck
        {
            AccountNumber = "24413815",
            RoutingNumber = "490000018",
            CheckType = checkTypeType.PERSONAL,
            SecCode = "PPD",
            AccountType = accountTypeType.CHECKING,
            CheckHolder = new HpsCheckHolder
            {
                Address = new HpsAddress {
                    Address = "6860 Dallas Parkway",
                    City = "Plano",
                    State = "TX",
                    Zip = "75024"
                },
                DlNumber = "1234567",
                DlState = "TX",
                FirstName = "John",
                LastName = "Doe",
                Phone = "1234567890",
                DobYear = 1997,
                Ssl4 = "4321"
            }
        },
        invalidCheckHolder = new HpsCheck
        {
            AccountNumber = "24413815",
            RoutingNumber = "490000018",
            CheckType = checkTypeType.PERSONAL,
            SecCode = "PPD",
            AccountType = accountTypeType.CHECKING,
            CheckHolder = new HpsCheckHolder
            {
                Address = new HpsAddress {
                    Address = "6860 Dallas Parkway",
                    City = "Plano",
                    State = "TX",
                    Zip = "75024"
                },
                DlNumber = "",
                DlState = "",
                FirstName = "John",
                LastName = "Doe",
                Phone = "",
                DobYear = 1997,
                Ssl4 = "4321"
            }
        },
        decline = new HpsCheck
        {
            AccountNumber = "24413815",
            RoutingNumber = "490000034",
            CheckType = checkTypeType.PERSONAL,
            SecCode = "PPD",
            AccountType = accountTypeType.CHECKING,
            CheckHolder = new HpsCheckHolder
            {
                Address = new HpsAddress
                {
                    Address = "6860 Dallas Parkway",
                    City = "Plano",
                    State = "TX",
                    Zip = "75024"
                },
                DlNumber = "1234567",
                DlState = "TX",
                FirstName = "John",
                LastName = "Doe",
                Phone = "1234567890",
                DobYear = 1997,
                Ssl4 = "4321"
            }
        },
        certification = new HpsCheck
        {
            AccountNumber = "24413815",
            RoutingNumber = "490000018",
            CheckType = checkTypeType.PERSONAL,
            SecCode = "PPD",
            AccountType = accountTypeType.CHECKING,
            CheckHolder = new HpsCheckHolder
            {
                Address = new HpsAddress
                {
                    Address = "123 Main St.",
                    City = "Downtown",
                    State = "NJ",
                    Zip = "12345"
                },
                DlNumber = "09876543210",
                DlState = "TX",
                FirstName = "John",
                LastName = "Doe",
                Phone = "8003214567",
                DobYear = 1997,
                Ssl4 = "4321"
            }
        };

        /// <summary>Gets a check that will approve.</summary>
        public static HpsCheck Approve
        {
            get
            {
                return approve;
            }
        }

        /// <summary>Gets a check that will throw an HPS Exception due to invalid check holder (empty fields).</summary>
        public static HpsCheck InvalidCheckHolder
        {
            get
            {
                return invalidCheckHolder;
            }
        }

        /// <summary>Gets a check that will decline.</summary>
        public static HpsCheck Decline
        {
            get
            {
                return decline;
            }
        }

        /// <summary>Gets a check that will decline.</summary>
        public static HpsCheck Certification
        {
            get
            {
                return certification;
            }
        }

    }
}
