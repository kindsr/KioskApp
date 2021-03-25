// BAGGAGE_VALIDATION_LANGUAGE.cs // 2020.07.08 13:00:00
// Copyright (c) 2020 iRobo Tech Corporation.
// A-808, 809 Tera Tower, Songpadaero 167, Songpa-gu, Seoul 05855 Korea
// All rights reserved.
//
// This software is the confidential and proprietary information of 
// iRobo Tech Corporation. ("Confidential Information").  You shall not
// disclose such Confidential Information and shall use it only in
// accordance with the terms of the license agreement you entered into
// with iRobo Tech Corporation.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iBeautyNail.Messages.Exceptions.Enums
{
    public enum VALIDATION_TITLE_MESSAGE
    {
        [Description("Error")]
        VALIDATION_TITLE_ERROR = 1,

        [Description("Information")]
        VALIDATION_TITLE_INFO = 2,

        [Description("Warning")]
        VALIDATION_TITLE_WARNNING = 3,
    }
    public enum VALIDATION_MESSAGE
    {
        [Description("Selected all designs.")]
        VALIDATION_FULL_DESIGN = 1,
        
        [Description("Confirm")]
        CONFIRM = 2,

        [Description("Would you like to print the receipt?")]
        VALIDATION_PRINT_RECEIPT = 3,

        [Description("Failed to read your card.")]
        VALIDATION_CARD_PAYMENT_FAILED = 4,

        [Description("It will be going to the first page.")]
        VALIDATION_FIRST_PAGE = 5,

        [Description("Printing the selected image. Please wait. (About 2 min)")]
        VALIDATION_WAIT_FOR_ADMIN = 6,

        [Description("The validity of the QR code has expired.")]
        VALIDATION_EXPIRED_QR_CODE = 7,

        [Description("It is not valid QR code.")]
        VALIDATION_NOT_VALID_QR_CODE = 8,

        [Description("Image downloading")]
        IMAGE_DOWNLOADING = 9,

        [Description("Failed to print nail sticker.")]
        VALIDATION_PRINT_FAILED = 10,

        [Description("Please call the manager in charge.")]
        CALL_THE_MANAGER = 11,

        [Description("Nail sticker label is empty.")]
        VALIDATION_EMPTY_PAPER = 12,

        [Description("Receipt paper is empty.")]
        VALIDATION_EMPTY_RECEIPT_PAPER = 13,

        [Description("If you push the picture, it is applied set designs recommended.")]
        M200_Comment1 = 14,

        [Description("You can select the nail design one by one.")]
        M200_Comment2 = 15,

        [Description("If you push the X button, it is able to insert another design.")]
        M200_Comment3 = 16,

        [Description("Look around the nail designs with various design theme.")]
        M200_Comment4 = 17,
    }
}
