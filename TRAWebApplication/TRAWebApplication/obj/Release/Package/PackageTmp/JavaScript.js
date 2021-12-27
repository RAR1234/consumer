///==================================================================================
/// Description: Function for show popup for add upload file
/// Created By: Nilesh
/// Event: Ribbon button
/// Created On  : 23-DEC-18
///===================================================================================

function OpenNotesPopup() {

    var windowOptions = { height: 400, width: 400 }
    var id = Xrm.Page.data.entity.getId();
    var customParameters = encodeURIComponent("id=" + id + "&typename=tra_tscsapplication");
    Xrm.Utility.openWebResource("dyn_DnDFileUploadArea.htm", customParameters, 400, 400);
}

///==================================================================================
/// Description: Function for filter complaint subtype lookup based on selected complaint type
/// Created By: Mani
/// Event: Form Onload
/// Created On  : 1-Apr-18
///===================================================================================

function ShowFixedProbsbtn() {
    debugger;
    var casetype = Xrm.Page.getAttribute("casetypecode").getValue();
    var complaintType = Xrm.Page.getAttribute("tra_complainttype").getValue();
    var complaintTypeID = complaintType[0].id;
    var complaintTypeName = complaintType[0].name;
    
    var IsCurrentuser = IsLoggedUsedForConsumerAffairs();

    if (casetype === 1 && complaintTypeName === "Quality of Service" && IsCurrentuser === true) {
        return true;
    } else {
        return false;
    }
   

    
}

function IsLoggedUsedForConsumerAffairs() {
    debugger;
    var IsCurrentUserLogin;
    var fetchXml = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" +
        "  <entity name='systemuser'>" +
        "    <attribute name='fullname' />" +
        "    <attribute name='businessunitid' />" +
        "    <attribute name='title' />" +
        "    <attribute name='address1_telephone1' />" +
        "    <attribute name='positionid' />" +
        "    <attribute name='systemuserid' />" +
        "    <order attribute='fullname' descending='false' />" +
        "    <filter type='and'>" +
        "      <condition attribute='systemuserid' operator='eq-userid' />" +
        "    </filter>" +
        "    <link-entity name='teammembership' from='systemuserid' to='systemuserid' visible='false' intersect='true'>" +
        "      <link-entity name='team' from='teamid' to='teamid' alias='ak'>" +
        "        <filter type='and'>" +
        "          <condition attribute='teamid' operator='eq' uiname='Consumer Affairs' uitype='team' value='{333C9D18-5333-E811-80CC-005056890167}' />" +
        "        </filter>" +
        "      </link-entity>" +
        "    </link-entity>" +
        "  </entity>" +
        "</fetch>";
    var req = new XMLHttpRequest();
    req.open("GET", Xrm.Page.context.getClientUrl() + "/api/data/v8.2/systemusers?fetchXml=" + fetchXml, false);
    req.setRequestHeader("OData-MaxVersion", "4.0");
    req.setRequestHeader("OData-Version", "4.0");
    req.setRequestHeader("Accept", "application/json");
    req.setRequestHeader("Prefer", "odata.include-annotations=\"*\"");
    req.onreadystatechange = function () {
        if (this.readyState === 4) {
            req.onreadystatechange = null;
            if (this.status === 200) {
                var results = JSON.parse(this.response);
                if (results.value.length > 0) {
                    IsCurrentUserLogin = true;
                   
                } else {
                    IsCurrentUserLogin = false;
                    
                }
            } else {
                Xrm.Utility.alertDialog(this.statusText);
            }
        }
    };
    req.send();
    return IsCurrentUserLogin;

}


function preFilterComplaintSubtypeLookup() {
    Xrm.Page.getControl("tra_complaintsubtype").addPreSearch(function () {
        addLookupFilterForComplaintSubtype();

    });
}
function addLookupFilterForComplaintSubtype() {

    var complaintType = Xrm.Page.getAttribute("tra_complainttype").getValue();
    var complaintTypeID = complaintType[0].id;
    var complaintTypeName = complaintType[0].name;

    if (complaintType != null) {

        fetchXml = "<filter type='and'><condition attribute='tra_complainttype' operator='eq' value='" + complaintTypeID + "' /></filter>"; Xrm.Page.getControl("tra_complaintsubtype").addCustomFilter(fetchXml);
    }
}

///===================================================================================
/// Description : Function for filter service lookup based on selected service provider
/// Created By : Mani
/// Event: Form Onload
/// Created On  : 1-Apr-18
///===================================================================================

function preFilterServiceLookup() {
    Xrm.Page.getControl("tra_service").addPreSearch(function () {
        addLookupFilterForService();

    });
}
function addLookupFilterForService() {

    var serviceProvider = Xrm.Page.getAttribute("tra_serviceprovider").getValue();
    var serviceProviderID = serviceProvider[0].id;
    var serviceProviderName = serviceProvider[0].name;

    if (serviceProvider != null) {

        fetchXml = "<filter type='and'><condition attribute='tra_serviceprovider' operator='eq' value='" + serviceProviderID + "' /></filter>"; Xrm.Page.getControl("tra_service").addCustomFilter(fetchXml);
    }
}

///==================================================================================
/// Description : Function for pop up value on Send For Consent Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 02-April-18
///===================================================================================
function SetConsentStatus() {
    try {
        if (Xrm.Page.getAttribute("tra_consentstatus") != null && Xrm.Page.getControl("header_statuscode") != null)
            var ConsentStatus = Xrm.Page.getAttribute("tra_consentstatus").getValue();
        //var Status = Xrm.Page.getControl("header_statuscode").getValue();
        var Status = Xrm.Page.getControl("header_statuscode").getAttribute().getValue();
        if (ConsentStatus == 3 && Status == 167490003) {

            Xrm.Utility.confirmDialog("The consumer will now be contacted for consent. Continue?", function () {

                Xrm.Page.getAttribute("tra_consentstatus").setValue(2);
                // Xrm.Page.getControl("header_statuscode").setValue(2);
                Xrm.Page.getAttribute("statuscode").setValue(2);
                Xrm.Page.data.entity.save();


            }, function () {



            });
        }
    }

    catch (err) {
        alert('Set Send For Consent function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Show Send For Consent Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function ShowSendForConsent() {
    try {
        if (Xrm.Page.getAttribute("tra_consentstatus") != null && Xrm.Page.getAttribute("casetypecode") != null && Xrm.Page.getControl("header_caseorigincode") != null)
            var ConsentStatus = Xrm.Page.getAttribute("tra_consentstatus").getValue();
        var CasetypeCode = Xrm.Page.getAttribute("casetypecode").getValue();
        var Origin = Xrm.Page.getControl("header_caseorigincode").getAttribute().getValue();
        if (Origin == 1 && CasetypeCode == 1 && ConsentStatus == 3) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Show Send For Consent function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Hide Send For Consent Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function HideSendForConsent() {
    try {
        if (Xrm.Page.getAttribute("tra_consentstatus") != null && Xrm.Page.getControl("header_statuscode") != null)
            var ConsentStatus = Xrm.Page.getAttribute("tra_consentstatus").getValue();
        var StatusCode = Xrm.Page.getControl("header_statuscode").getAttribute().getValue();
        if (ConsentStatus == 2 && StatusCode == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('Hide Send For Consent function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Function for pop up value on Accept Scope Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function SetAcceptScope() {
    try {
        if (Xrm.Page.getAttribute("tra_scopeacceptance") != null)
            var AcceptScope = Xrm.Page.getAttribute("tra_scopeacceptance").getValue();
        if (AcceptScope == null || AcceptScope == "") {

            Xrm.Utility.confirmDialog("Confirm Scope Acceptance?", function () {

                Xrm.Page.getAttribute("tra_scopeacceptance").setValue(1);
                Xrm.Page.getAttribute("statuscode").setValue(167490008);
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_scopeacceptance").setValue(null);

            });
        }
    }

    catch (err) {
        alert('Set Accept Scope function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Show Accept Scope  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function showAcceptScope() {
    try {
        if (Xrm.Page.getAttribute("tra_consentstatus") != null && Xrm.Page.getAttribute("casetypecode") != null)
            var ConsentStatus = Xrm.Page.getAttribute("tra_consentstatus").getValue();
        var CasetypeCode = Xrm.Page.getAttribute("casetypecode").getValue();
        var CaseOrigin = Xrm.Page.getControl("header_caseorigincode").getAttribute().getValue();
        if (ConsentStatus == 1 && (CasetypeCode == 1)) {
            return true;
        }
        if (CasetypeCode == 3 || CasetypeCode == 2) {
            return false;
        }
        if (CaseOrigin == 2 || CaseOrigin == 3 || CaseOrigin == 4 || CaseOrigin == 5) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('show Accept Scope function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Hide Accept and Reject Scope  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function HideAcceptNrejectScope() {
    try {
        if (Xrm.Page.getAttribute("tra_scopeacceptance") != null)
            var ScopeAccepted = Xrm.Page.getAttribute("tra_scopeacceptance").getValue();
        if (ScopeAccepted == 1 || ScopeAccepted == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('show Accept Scope function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Function for pop up value on Reject Scope Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function SetRejectScope() {
    try {
        if (Xrm.Page.getAttribute("tra_scopeacceptance") != null)
            var AcceptScope = Xrm.Page.getAttribute("tra_scopeacceptance").getValue();
        if (AcceptScope == null || AcceptScope == "") {

            Xrm.Utility.confirmDialog("Confirm Scope Rejection?", function () {

                Xrm.Page.getAttribute("tra_scopeacceptance").setValue(2);
                Xrm.Page.getAttribute("statuscode").setValue(167490009);
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_scopeacceptance").setValue(null);

            });
        }
    }


    catch (err) {
        alert('Set Reject Scope function Error:' + err.description);
    }
}
///==================================================================================
/// Description : Show Reject Scope  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function showRejectScope() {
    try {
        if (Xrm.Page.getAttribute("tra_consentstatus") != null && Xrm.Page.getAttribute("casetypecode") != null)
            var ConsentStatus = Xrm.Page.getAttribute("tra_consentstatus").getValue();
        var CasetypeCode = Xrm.Page.getAttribute("casetypecode").getValue();
        var CaseOrigin = Xrm.Page.getControl("header_caseorigincode").getAttribute().getValue();
        if (ConsentStatus == 1 && CasetypeCode == 1) {
            return true;
        }
        if (CasetypeCode == 2 || CasetypeCode == 3) {
            return false;
        }
        if (CaseOrigin == 2 || CaseOrigin == 3 || CaseOrigin == 4 || CaseOrigin == 5) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('show Reject Scope function Error:' + err.description);
    }
}


///==================================================================================
/// Description : Show Documents Verified Btn
/// Created By  :  Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function DocumentsVerified() {
    try {
        if (Xrm.Page.getAttribute("tra_scopeacceptance") != null)
            var DocumentsVerified = Xrm.Page.getAttribute("tra_scopeacceptance").getValue();
        if (DocumentsVerified == 1) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Documents Verified function Error:' + err.description);
    }
}

///===================================================================================
/// Description : Function for pop up value on Documents Verified Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function SetDocumentsVerified() {
    try {
        if (Xrm.Page.getAttribute("tra_documentsverified") != null)
            var AcceptScope = Xrm.Page.getAttribute("tra_documentsverified").getValue();
        if (AcceptScope == null || AcceptScope == "" || AcceptScope == 1) {

            Xrm.Utility.confirmDialog("This case will now be assigned to Consumer Affairs. Continue?", function () {

                Xrm.Page.getAttribute("tra_documentsverified").setValue(2);
                Xrm.Page.getAttribute("statuscode").setValue(167490010);
                Xrm.Page.getAttribute("tra_inforequiredca").setValue();
                Xrm.Page.getAttribute("tra_inforequired").setValue();
                Xrm.Page.getAttribute("tra_assigntodepartment").setValue();
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_documentsverified").setValue(null);

            });
        }
    }

    catch (err) {
        alert('Set Documents Verified function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Show Assign to Service Provider Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function AssigntoServiceProvider() {
    try {
        if (Xrm.Page.getAttribute("tra_documentsverified") != null)
            var AssigntoServiceProvider = Xrm.Page.getAttribute("tra_documentsverified").getValue();
        // var AssigntoDepartment = Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").getValue();
        if (AssigntoServiceProvider == 2) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Assign to Service Provider function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Hide Documents Verified  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function HideDocumentsVerified() {
    try {
        if (Xrm.Page.getAttribute("tra_documentsverified") != null)
            var ScopeAccepted = Xrm.Page.getAttribute("tra_documentsverified").getValue();
        if (ScopeAccepted == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('show Accept Scope function Error:' + err.description);
    }
}


///==================================================================================
/// Description : Show Assign To Consumer Affairs Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 05-Apr-18
///===================================================================================
function ShowAssignToConsumerAffairs() {
    try {
        if (Xrm.Page.getAttribute("tra_inforequired") != null || Xrm.Page.getAttribute("tra_inforequiredca") != null
            || Xrm.Page.getAttribute("tra_assigntodepartment") != null)
            var InfoRequired = Xrm.Page.getAttribute("tra_inforequired").getValue();
        var InfoRequiredca = Xrm.Page.getAttribute("tra_inforequiredca").getValue();
        var issuedecision = Xrm.Page.getAttribute("tra_issuedecision").getValue();
        var AssignToDepartment = Xrm.Page.getAttribute("tra_assigntodepartment").getValue();
        if (InfoRequired == 2 || InfoRequiredca == 2 || AssignToDepartment == 2 || issuedecision == 2) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Show Assign to Consumer Affairs function Error:' + err.description);
    }
}


///==================================================================================
/// Description : Hide Assign to Department  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function HideAssigntoDepartment() {
    try {
        if (Xrm.Page.getAttribute("tra_assigntodepartment") != null)
            var AssigntoDepartment = Xrm.Page.getAttribute("tra_assigntodepartment").getValue();
        if (AssigntoDepartment == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('Hide Assign to Department function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Hide Assign to Department  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 05-Apr-18
///===================================================================================
function HideAdditonalInfoReqd() {
    try {
        if (Xrm.Page.getAttribute("tra_inforequired") != null || Xrm.Page.getAttribute("tra_inforequiredca") != null)
            var AdditonalInfoReqd = Xrm.Page.getAttribute("tra_inforequired").getValue();
        var AdditonalInfoReqdCA = Xrm.Page.getAttribute("tra_inforequiredca").getValue();
        if (AdditonalInfoReqd == 2 || AdditonalInfoReqdCA == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('Hide Additonal Info Reqd function Error:' + err.description);
    }
}
///==================================================================================
/// Description : Hide Reject Case  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function HideRejectCase() {
    try {
        var formtype = Xrm.Page.ui.getFormType();
        if (formtype == 1 || formtype == 2) {
            return false;
        }
        else {
            return true;

        }
    }
    catch (err) {
        alert('Hide Reject Case function Error:' + err.description);
    }
}

///===================================================================================
/// Description : Hide Issue Decision  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function HideIssueDecision() {
    try {
        var formtype = Xrm.Page.ui.getFormType();
        if (formtype == 1) {
            return false;
        }
        else {
            return true;

        }
    }
    catch (err) {
        alert('Hide Issue Decision function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Hide Issue Decision  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 29-Mar-18
///===================================================================================
function HideAcceptandRejectBtn() {
    try {
        var formtype = Xrm.Page.ui.getFormType();
        if (formtype == 1) {
            return false;
        }
        else {
            return true;

        }
    }
    catch (err) {
        alert('Hide Issue Decision function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Set value in Header Status
/// Created By  : Sameer
/// Event       : OnChange(Consend Status)
/// Created On  : 03-April-18
///===================================================================================
function SetHeaderStatus() {
    try {

        var ConsendStatus = Xrm.Page.getAttribute("tra_consentstatus").getValue();
        if (ConsendStatus == 1) {
            Xrm.Page.getAttribute("statuscode").setValue(167490006);
            Xrm.Page.data.entity.save();
        }

    }
    catch (error) {
        alert("Set Header Status function Error " + error.message);
    }

}


///==================================================================================
/// Description : Set value of Department and Pop up 
/// Created By  : Sameer
/// Event       : Ribbon Js
/// Created On  : 04-April-18
///===================================================================================
function SetDepartment() {
    try {
        if (Xrm.Page.getAttribute("tra_department") != null)
            var Department = Xrm.Page.getAttribute("tra_department").getValue();
        //var name = Xrm.Page.getAttribute("tra_department").getValue()[0].name;
        if (Department == null) {
            Xrm.Utility.confirmDialog("Please select a department!", function () {
                Xrm.Page.ui.controls.get("tra_department").setFocus();
            },
                function () {
                    Xrm.Page.ui.controls.get("tra_department").setFocus();


                });

        }
        if (Department != null) {
            var name = Xrm.Page.getAttribute("tra_department").getValue()[0].name;
            Xrm.Utility.confirmDialog("This case will now be assigned to '" + name + "' department. Continue?", function () {

                Xrm.Page.getAttribute("tra_assigntodepartment").setValue(2);
                Xrm.Page.getAttribute("statuscode").setValue(167490018);
                Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue();
                Xrm.Page.data.entity.save();


            }, function () {

                // Xrm.Page.getAttribute("tra_department").setValue(null);

            });
        }
    }
    catch (err) {
        alert('Set Department function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Function for pop up value on Add'l Info. Reqd. Btn 
/// Created By  : Sameer
/// Event       : Ribbon Js
/// Created On  : 04-April-18
///===================================================================================
function SetAdditionalInfoReqd() {
    try {
        if (Xrm.Page.getAttribute("tra_inforequired") != null)
            var InfoRequired = Xrm.Page.getAttribute("tra_inforequired").getValue();
        if (InfoRequired == null || InfoRequired == "") {

            Xrm.Utility.confirmDialog("This case will now be assigned to Call Center. Continue?", function () {
                Xrm.Page.getAttribute("tra_inforequired").setValue(2);
                //Xrm.Page.getAttribute("statuscode").setValue(167490019);
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_inforequired").setValue(null);

            });
        }
    }

    catch (err) {
        alert('Set Additional Info Reqd function Error:' + err.description);
    }
}

///==================================================================================
/// Description : show Hidden field and set Status in Header
/// Created By  : Sameer
/// Event       : OnChange(Info Required)
/// Created On  : 04-April-18
///===================================================================================
function HideSetAdditionalInfo() {
    try {

        var InfoRequired = Xrm.Page.getAttribute("tra_inforequired").getValue();
        var InfoRequiredCA = Xrm.Page.getAttribute("tra_inforequiredca").getValue();
        if (InfoRequired == 2 || InfoRequiredCA == 2) {
            Xrm.Page.ui.controls.get("tra_inforequestedby").setVisible(true);
            Xrm.Page.getAttribute("statuscode").setValue(167490019);
            Xrm.Page.data.entity.save();
        }
        else {
            Xrm.Page.ui.controls.get("tra_inforequestedby").setVisible(false);
        }
    }
    catch (error) {
        alert("Set Header Status function Error " + error.message);
    }

}


///===================================================================================
/// Description: Function for filter Service subtype lookup based on selected service and service provider type
/// Created By: Mani
/// Event: Form Onload
/// Created On  : 5-Apr-18
///===================================================================================

function preFilterServiceSubtypeLookup() {
    Xrm.Page.getControl("tra_servicesubtype").addPreSearch(function () {
        addLookupFilterForServiceSubtype();

    });
}

function addLookupFilterForServiceSubtype() {
    var serviceType = Xrm.Page.getAttribute("tra_service").getValue();
    var serviceProvider = Xrm.Page.getAttribute("tra_serviceprovider").getValue();
    if (serviceType != null && serviceProvider != null) {
        var serviceTypeID = serviceType[0].id;
        var serviceTypeName = serviceType[0].name;
        var serviceProviderID = serviceProvider[0].id;
        var serviceProviderName = serviceProvider[0].name;
        var fetchXml =
            "<filter type='and'>" + "<condition attribute='tra_serviceprovider' operator='eq' value='" + serviceProviderID + "' />"
            + "<condition attribute='tra_servicetype' operator='eq' value='" + serviceTypeName + "' />" + "</filter>";
        Xrm.Page.getControl("tra_servicesubtype").addCustomFilter(fetchXml);
    }
}

///==================================================================================
/// Description : Show Add'l Info Reqd.(By CA)  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 06-Apr-18
///===================================================================================
function ShowAdditionalInfoReqd() {
    try {
        /// changes for Enquiry CR
        var CaseType = Xrm.Page.getAttribute("casetypecode").getValue();

        if (CaseType == 1) {
            if (Xrm.Page.getAttribute("tra_assigntodepartment") != null)
                var AssigntoDepartment = Xrm.Page.getAttribute("tra_assigntodepartment").getValue();
            if (AssigntoDepartment == 2) {
                return true;
            }

            /// changes for Enquiry CR
            else {
                return false;
            }

        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Show Additional Info Reqd function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Function for pop up value on Add'l Info. Reqd.(CA) Btn 
/// Created By  : Sameer
/// Event       : Ribbon Js
/// Created On  : 05-April-18
///===================================================================================
function SetAdditionalInfoReqdCA() {
    try {
        if (Xrm.Page.getAttribute("tra_inforequiredca") != null)
            var InfoRequiredca = Xrm.Page.getAttribute("tra_inforequiredca").getValue();
        if (InfoRequiredca == null || InfoRequiredca == "") {

            Xrm.Utility.confirmDialog("This case will now be assigned to Call Center. Continue?", function () {
                Xrm.Page.getAttribute("tra_inforequiredca").setValue(2);
                //  Xrm.Page.getAttribute("statuscode").setValue(167490019);
                Xrm.Page.data.entity.save();

                //var CaseAttributes = Xrm.Page.data.entity.attributes.get();
                //    if (CaseAttributes != null) {
                //    for (var i in CaseAttributes) {
                //        if (CaseAttributes[i].getIsDirty()) {
                //            listofDirtyAttri += CaseAttributes[i].getName() + "\n";
                //        }
                //        Xrm.Page.data.entity.save();
                //    }
                //}
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_inforequiredca").setValue(null);

            });
        }
    }

    catch (err) {
        alert('Set Additional Info Reqd CA function Error:' + err.description);
    }
}

///==================================================================================
/// Description : hide Add'l Info Reqd.(By Dpt)  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 06-Apr-18
///===================================================================================
function HideAdditionalInfoReqdDpt() {
    try {
        if (Xrm.Page.getAttribute("tra_inforequired") != null)
            var InfoRequired = Xrm.Page.getAttribute("tra_inforequired").getValue();
        if (InfoRequired == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('Show Additional Info Reqd function Error:' + err.description);
    }
}

///==================================================================================
/// Description : hide Add'l Info Reqd.(By CA) & Assign to service provider Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 09-Apr-18
///===================================================================================
function HideAdditionalInfoReqdCAandAssigntoDept() {
    try {
        if (Xrm.Page.getAttribute("tra_assigntodepartment") != null)
            var AssigntoDepartment = Xrm.Page.getAttribute("tra_assigntodepartment").getValue();
        if (AssigntoDepartment == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('Hide Additional Info ReqdCA and Assign to Dept function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Function for pop up value on Assign to service provider Btn 
/// Created By  : Sameer
/// Event       : Ribbon Js
/// Created On  : 09-April-18
///===================================================================================
function SetAssignToServiceProvider() {

    try {

        var operatorslafailedvar = Xrm.Page.getAttribute("tra_operatorslafailed").getValue();
        if (operatorslafailedvar == 0) {
            Xrm.Page.getAttribute("tra_operatorslafailed").setValue(1);
        }

        if (Xrm.Page.getAttribute("tra_assigntosp").getValue() != null)
            var InfoRequiredca = Xrm.Page.getAttribute("tra_assigntosp").getValue();
        var AssignmentCount = Xrm.Page.getAttribute("tra_assignmentcount").getValue();
        if (InfoRequiredca == null || InfoRequiredca == "") {

            Xrm.Utility.confirmDialog("This case will now be assigned to Service Provider. Continue?", function () {
                Xrm.Page.getAttribute("tra_assigntosp").setValue(2);
                if (AssignmentCount == null || AssignmentCount == "") {
                    Xrm.Page.getAttribute("statuscode").setValue(3);
                    Xrm.Page.getAttribute("tra_assignmentcount").setValue(1);
                    Xrm.Page.data.entity.save();
                }
                else if (AssignmentCount != null || AssignmentCount != "")
                    Xrm.Page.getAttribute("statuscode").setValue(167490015);
                AssignmentCount++;
                Xrm.Page.getAttribute("tra_assignmentcount").setValue(AssignmentCount);
                Xrm.Page.data.entity.save();


            },
                function () {

                    Xrm.Page.getAttribute("tra_assigntosp").setValue(null);

                });
        }
    }

    catch (err) {
        alert('Set Assign To Service Provider function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Function for pop up value on Consumer Affairs Btn 
/// Created By  : Sameer
/// Event       : Ribbon Js
/// Created On  : 10-April-18
///===================================================================================
function SetConsumerAffairs() {
    try {
        debugger;
        if (Xrm.Page.getAttribute("tra_assigntoconsumeraffairs") != null)
            var AssignToConsumerAffairs = Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").getValue();
        var caseType = Xrm.Page.getAttribute("casetypecode").getValue();

        var SettelmentLtr = Xrm.Page.getAttribute("tra_settlementletterrequested").getValue();
        if (AssignToConsumerAffairs == null || AssignToConsumerAffairs == "" || AssignToConsumerAffairs == 1) {
            Xrm.Utility.confirmDialog("This case will now be assigned to Consumer Affairs. Continue?", function () {
                if (caseType == 2 || caseType == 3) {
                    debugger;
                    Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue(2);
                }
                else {
                    Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue(2);
                    //  Xrm.Page.getAttribute("statuscode").setValue(167490013);
                    Xrm.Page.getAttribute("statuscode").setValue(167490017);
                    Xrm.Page.getAttribute("tra_assigntodepartment").setValue();
                    Xrm.Page.getAttribute("tra_inforequiredca").setValue();//17-Apr-18
                    Xrm.Page.getAttribute("tra_inforequired").setValue();//17-Apr-18
                    Xrm.Page.getAttribute("tra_issuedecision").setValue();//17-Apr-18
                    //Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue();//17-Apr-18

                    //Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").setValue(true); //17-June-2020


                    if (SettelmentLtr == 0) {
                        Xrm.Page.getAttribute("tra_notificationstatus").setValue();
                        Xrm.Page.getAttribute("tra_satisfactionlevel").setValue();
                    }
                }
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue(null);

            });
        }
    }

    catch (err) {
        alert('Set Consumer Affairs function Error:' + err.description);
    }
}

///==================================================================================
/// Description : hide Assign to Consumer Affairs & Add'l Info Reqd.(By Dpt) Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 10-Apr-18
///===================================================================================
function HideAssigntoconsumerNaddlinforeqd() {
    try {

        var AssigntoDepartment = Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").getValue();
        var caseType = Xrm.Page.getAttribute("casetypecode").getValue();
        if (AssigntoDepartment == 2) {
            return false;
        }

        else {
            return true;
        }
    }
    catch (err) {
        alert('Hide Assign to consumer N addl info reqd function Error:' + err.description);
    }
}

///==================================================================================
/// Description :Show Assign to service provider & Other Dept  Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 10-Apr-18
///===================================================================================
function ShowAssigntoSPnOtherDepartment() {
    try {
        if (Xrm.Page.getAttribute("tra_assigntoconsumeraffairs") != null || Xrm.Page.getAttribute("tra_documentsverified") != null)
            var AssigntoDepartment = Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").getValue();
        var DocumentsVerified = Xrm.Page.getAttribute("tra_documentsverified").getValue();
        if (AssigntoDepartment == 2 || DocumentsVerified == 2) {
            return true;
        }

        else {
            return false;
        }
    }
    catch (err) {
        alert('Show Assign to SP n Other Department function Error:' + err.description);
    }
}

///==================================================================================
/// Description :Show Issue Decision Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 10-Apr-18
///===================================================================================
function ShowIssueDecision() {
    try {

        var AssigntoDepartment = Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").getValue();
        var consumeraffairsflagenquiry = Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").getValue();
        var caseType = Xrm.Page.getAttribute("casetypecode").getValue();
        //var DocumentsVerified = Xrm.Page.getAttribute("tra_documentsverified").getValue(); //17-Apr-18

        if (AssigntoDepartment == 2) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Show Issue Decision function Error:' + err.description);
    }
}
function hideshowFeedback() {
    var consumeraffairsflagenquiry = Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").getValue();
    var caseType = Xrm.Page.getAttribute("casetypecode").getValue();
    var statusReason = Xrm.Page.getAttribute("statuscode").getValue();
    if (consumeraffairsflagenquiry == true && caseType == 2 && statusReason != 167490027) {
        return true;
    }
    //Added in 22 dec 2020//
    else if (consumeraffairsflagenquiry == true && caseType == 3 && statusReason != 167490027) {
        return true;
    }
    else {
        return false;
    }
}
///==================================================================================
/// Description : Hide Assign Service Provider Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 10-Apr-18
///===================================================================================
function HideAssigntoServiceProvider() {
    try {
        if (Xrm.Page.getAttribute("tra_assigntosp") != null)
            var Assigntosp = Xrm.Page.getAttribute("tra_assigntosp").getValue();
        if (Assigntosp == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('Hide Assign to Service Provider function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Pop up value on Issue decision
/// Created By  : Sameer
/// Event       : OnLoad
/// Created On  : 11-Apr-18 tra_finaldecision
///===================================================================================
function SetfinalDecision() {
    try {
        if (Xrm.Page.getAttribute("tra_finaldecision") != null)
            var FinalDecision = Xrm.Page.getAttribute("tra_finaldecision").getValue();
        if (FinalDecision == null) {
            Xrm.Utility.confirmDialog("Please provide a final Decision!", function () {
                Xrm.Page.ui.controls.get("tra_finaldecision").setVisible(true);
                Xrm.Page.ui.controls.get("tra_finaldecision").setFocus();
            },
                function () {
                    // Xrm.Page.ui.controls.get("tra_department").setFocus();


                });

        }
        if (FinalDecision != null) {
            Xrm.Utility.confirmDialog("This case will now be assigned to Call Center for notifying the Consumer. Continue?", function () {

                Xrm.Page.getAttribute("tra_issuedecision").setValue(2);
                Xrm.Page.getControl("tra_finaldecision").setDisabled(true);
                Xrm.Page.getAttribute("statuscode").setValue(167490014);
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_issuedecision").setValue(null);

            });
        }
    }
    catch (err) {
        alert('Set final Decision function Error:' + err.description);
    }
}

///==================================================================================
/// Description :Show Notify Consumer & Consumer Affairs Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 11-Apr-18
///===================================================================================
function ShowNotifyConsumerNConsumerAffairs() {
    try {
        if (Xrm.Page.getAttribute("tra_issuedecision") != null)
            var IssueDecision = Xrm.Page.getAttribute("tra_issuedecision").getValue();
        if (IssueDecision == 2) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Show Notify Consumer N Consumer Affairs function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Hide Issue Decision Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 11-Apr-18
///===================================================================================
function HideIssueDecision1() {
    try {
        if (Xrm.Page.getAttribute("tra_issuedecision") != null)
            var IssueDecision = Xrm.Page.getAttribute("tra_issuedecision").getValue();
        // var DocumentsVerified = Xrm.Page.getAttribute("tra_documentsverified").getValue();
        if (IssueDecision == 2) {
            return false;
        }
        else {
            return true;
        }
    }
    catch (err) {
        alert('Hide Issue Decision function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Popup value through HTML web resource
/// Created By  : Sameer
/// update      :Payal 
/// Event       : Ribbon Btn
/// Created On  : 12-Apr-18
///Updated on   : 25-Apr-18
///===================================================================================
function htmlIssueDecision() {
    try {
        //  var callbackRef = function(r){alert(r)};
        //instantiate dialog

        //set callback to execute when selection is made and dialog closes

        var caseid = Xrm.Page.data.entity.getId();
        var PassValues = encodeURIComponent(caseid);
        var webresourceurl = "/WebResources/tra_CaseHtml?Data=" + PassValues;
        //   var returnedValue = Xrm.Utility.openWebResource("tra_CaseHtml", PassValues);
        //   if (returnedValue == "ok") {
        //      window.location.href = window.location.protocol + "//" + window.location.host + window.location.pathname + window.location.search;
        var DialogOption = new Xrm.DialogOptions();
        DialogOption.width = 500;
        DialogOption.height = 500;
        //var dialogWindow = new window.top.Mscrm.CrmDialog(Mscrm.CrmUri.create(webresourceurl), window, 500, 500);
        //dialogWindow.setCallbackReference(CallbackFunction);
        //dialogWindow.show();
        window.parent.Xrm.Internal.openDialog(Mscrm.CrmUri.create(webresourceurl).toString(),
            DialogOption,
            null, null,
            CallbackFunction);


    }

    catch (err) {
        alert('html Issue Decision function Error:' + err.description);
    }
}
function CallbackFunction(result) {
    Xrm.Page.getAttribute("tra_finaldecision").setValue(result);
    Xrm.Page.getAttribute("tra_finaldecision").setSubmitMode("always");
    Xrm.Page.getAttribute("tra_issuedecision").setValue(2);
    Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue(null);
    var caseType = Xrm.Page.getAttribute("casetypecode").getValue();
    if (caseType == 1) {
        Xrm.Page.getAttribute("tra_documentsverified").setValue(2);
        Xrm.Page.ui.controls.get("tra_finaldecision").setVisible(true);
        Xrm.Page.ui.controls.get("tra_notificationstatus").setVisible(true);
        Xrm.Page.ui.controls.get("tra_satisfactionlevel").setVisible(true);
        Xrm.Page.getControl("header_statuscode").getAttribute().setValue(167490014);
    }
    else if (caseType == 2) {
        Xrm.Page.getControl("header_statuscode").getAttribute().setValue(167490027);
        Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue(1);
        Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").setValue(false);
        /// change after Enquiry CR
        /// Set IsFeedbackProvided = Yes
        Xrm.Page.getAttribute("tra_isfeedbackprovided").setValue(1);
    }
    //Added in 22 dec 2020//
    else if (caseType == 3) {
        Xrm.Page.getControl("header_statuscode").getAttribute().setValue(167490027);
        Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue(1);
        Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").setValue(false);
        /// change after Enquiry CR
        /// Set IsFeedbackProvided = Yes
        Xrm.Page.getAttribute("tra_isfeedbackprovided").setValue(1);
    }

    Xrm.Page.data.entity.save();

}
///==================================================================================
/// Description : Hide Business Process flow
/// Created By  : Sameer
/// Event       : OnLoad
/// Created On  : 13-Apr-18
///===================================================================================
function collapseOpporBusinessProcess() {

    Xrm.Page.ui.process != null && Xrm.Page.ui.process.setDisplayState("collapsed");

}
///==================================================================================
/// Description : Show field final Decision
/// Created By  : Sameer
/// Event       : OnLoad(Form when issue decision == yes)
/// Created On  : 13-Apr-18
///===================================================================================
function ShowFinalDecision() {
    try {
        if (Xrm.Page.getAttribute("tra_issuedecision") != null || Xrm.Page.getAttribute("header_statuscode") != null
            || Xrm.Page.getAttribute("tra_notificationstatus") != null || Xrm.Page.getAttribute("tra_satisfactionlevel") != null)
            var IssueDecision = Xrm.Page.getAttribute("tra_issuedecision").getValue();
        if (IssueDecision == 2) {
            Xrm.Page.ui.controls.get("tra_finaldecision").setVisible(true);
            //        Xrm.Page.getAttribute("statuscode").setValue(167490014);
            Xrm.Page.ui.controls.get("tra_notificationstatus").setVisible(true);
            Xrm.Page.ui.controls.get("tra_satisfactionlevel").setVisible(true);

            //Xrm.Page.getAttribute("tra_notificationstatus").setRequiredLevel("required");
            // Xrm.Page.ui.controls.get("tra_notificationstatus").setRequiredLevel("required");
            Xrm.Page.data.entity.save();
        }
    }

    catch (err) {
        alert('Show Final Decision function Error:' + err.description);
    }
}


///==================================================================================
/// Description : Show Assign To Consumer Affairs Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 15-Apr-18
///===================================================================================
function ShowAssignToConsumerAffairs1() {
    try {
        if (Xrm.Page.getAttribute("tra_issuedecision") != null || Xrm.Page.getAttribute("tra_assigntodepartment") != null)
            var IssueDecision = Xrm.Page.getAttribute("tra_issuedecision").getValue();//15-Apr-18
        var AssigntoDepartment = Xrm.Page.getAttribute("tra_assigntodepartment").getValue();
        var caseType = Xrm.Page.getAttribute("casetypecode").getValue();
        if ((IssueDecision == 2 || AssigntoDepartment == 2) && caseType == 1) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Show Assign to Consumer Affairs1 function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Date of Complaint Validation
/// Created By  : Sameer
/// Event       : OnChange (Date of Complaint against SP)
/// Created On  : 16-Apr-18
///===================================================================================
function DatePublishValidation() {
    try {
        var dateofcomplaint = Xrm.Page.getAttribute("tra_dateofcomplaintagainstserviceprovider").getValue();

        if (dateofcomplaint != null) {
            var today = new Date();
            var todayyear = today.getFullYear() + "";
            var todaymonth = (today.getMonth() + 1) + "";
            var todayday = today.getDate() + "";

            var dateofcomplaintyear = dateofcomplaint.getFullYear() + "";
            var dateofcomplaintmonth = (dateofcomplaint.getMonth() + 1) + "";
            var dateofcomplaintday = dateofcomplaint.getDate() + "";

            var todaydate = new Date(todayyear, todaymonth, todayday);
            var dateofcomplaint1 = new Date(dateofcomplaintyear, dateofcomplaintmonth, dateofcomplaintday);

            if (dateofcomplaint1 > todaydate) {
                Xrm.Page.getAttribute("tra_dateofcomplaintagainstserviceprovider").setValue(null);
                Xrm.Page.getControl("tra_dateofcomplaintagainstserviceprovider").setNotification("Date of Complaint should not be greater than today.");
            }
            else {
                Xrm.Page.getControl("tra_dateofcomplaintagainstserviceprovider").clearNotification();
            }
        }
    }
    catch (error) {
        alert("Error occured in Date Publish Validation function.... Please contact your administrator... " + error.message);
    }
}

///==================================================================================
/// Description : Show Assign To Consumer Affairs Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 16-Apr-18
///===================================================================================
function ShowAssignToCAonSatisfactionLevel() {
    try {
        if (Xrm.Page.getAttribute("tra_satisfactionlevel") != null)
            var SatisfactionLevel = Xrm.Page.getAttribute("tra_satisfactionlevel").getValue();

        if (SatisfactionLevel == 167490003 || SatisfactionLevel == 167490004) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('Show Assign to Consumer Affairs1 function Error:' + err.description);
    }
}


///===================================================================================
/// Description : Function to change label of description box and set visibility of case questions section
/// Created By : Mani
/// Event: Form Onload/Change of complaint type
/// Created On  : 19-Apr-18
///===================================================================================

function onChangeOfCaseType() {
    var caseType = Xrm.Page.getAttribute("casetypecode").getValue();
    var caseOrigin = Xrm.Page.getAttribute("caseorigincode").getValue();
    if (caseOrigin == 1 || caseOrigin == 3) {
        if (caseType != null) {
            if (caseType == 2) {
                Xrm.Page.getControl("description").setLabel("Enquiry");
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_CaseQuestions").setVisible(false);
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_EnquirySuggestion").setVisible(true);
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_ServiceInformation").setVisible(false);
                Xrm.Page.getAttribute("tra_casequestion1").setRequiredLevel("none");
                Xrm.Page.getAttribute("tra_casequestion2").setRequiredLevel("none");
                Xrm.Page.getAttribute("tra_casequestion3").setRequiredLevel("none");
            }
            else if (caseType == 3) {
                Xrm.Page.getControl("description").setLabel("Suggestion");
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_CaseQuestions").setVisible(false);
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_EnquirySuggestion").setVisible(true);
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_ServiceInformation").setVisible(false);
                Xrm.Page.getAttribute("tra_casequestion1").setRequiredLevel("none");
                Xrm.Page.getAttribute("tra_casequestion2").setRequiredLevel("none");
                Xrm.Page.getAttribute("tra_casequestion3").setRequiredLevel("none");
            }
            else if (caseType == 1) {
                Xrm.Page.getControl("description").setLabel("Description/Addl. Comments");
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_CaseQuestions").setVisible(true);
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_EnquirySuggestion").setVisible(true);
                Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_ServiceInformation").setVisible(true);
                Xrm.Page.getAttribute("tra_casequestion1").setRequiredLevel("required");
                Xrm.Page.getAttribute("tra_casequestion2").setRequiredLevel("required");
                Xrm.Page.getAttribute("tra_casequestion3").setRequiredLevel("required");
            }
        }
    }
    else if (caseOrigin == 2 || caseOrigin == 4) {
        Xrm.Page.getControl("description").setLabel("Description");
        Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_CaseQuestions").setVisible(false);
        Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Section_EnquirySuggestion").setVisible(true);
        Xrm.Page.getAttribute("tra_casequestion1").setRequiredLevel("none");
        Xrm.Page.getAttribute("tra_casequestion2").setRequiredLevel("none");
        Xrm.Page.getAttribute("tra_casequestion3").setRequiredLevel("none");
    }
}

function updateprocesscase() {
    try {
        var status = Xrm.Page.getAttribute("statuscode").getValue();
        if (status == 167490017 || status == 167490010) {
            Xrm.Page.getAttribute("statuscode").setValue(167490001);
            Xrm.Page.data.entity.save();
        }
    }
    catch (err) {
        alert('updateprocesscase function Error:' + err.description);
    }
}
///===================================================================================
/// Description : Function to show settlement letter Requested field when status is issue requested
/// Created By : Payal
/// Event: Form Onload/Change of status
/// Created On  : 24-Apr-18
///===================================================================================
function showSettlementreqLetter() {
    try {
        var status = Xrm.Page.getAttribute("statuscode").getValue();
        if (status == 167490014) {
            Xrm.Page.ui.controls.get("tra_settlementletterrequested").setVisible(true);
            Xrm.Page.getAttribute("tra_issuedecision").setValue(2);
            Xrm.Page.data.entity.save();
        }
        else {
            // Xrm.Page.ui.controls.get("tra_settlementletterrequested").setVisible(false);
        }
    }
    catch (err) {
        alert('showSettlementreqLetter function Error:' + err.description);
    }
}
function setSettlementreqLetter() {
    try {
        var letterRequset = Xrm.Page.getAttribute("tra_settlementletterrequested").getValue();
        if (letterRequset == true || letterRequset != "") {
            Xrm.Page.getAttribute("statuscode").setValue(167490023);
            Xrm.Page.data.entity.save();
        }

    }
    catch (err) {
        alert('showSettlementreqLetter function Error:' + err.description);
    }
}
///==================================================================================
/// Description : Hide processcase button
/// Created By  : Payal
/// Event       : Ribbon Btn
/// Created On  : 26-Apr-18
///===================================================================================
function hideProcesscase() {
    try {
        var finaldecision = Xrm.Page.getAttribute("tra_finaldecision").getValue();
        if (finaldecision == null) {
            return true;
        }
        else {
            return false;
        }
    }

    catch (err) {
        alert('hideProcesscase function Error:' + err.description);
    }
}


// JavaScript source code
///==================================================================================
/// Description: Function to strip HTML from description of case from email.
/// Created By: Mani
/// Event: Form Onload
/// Created On  : 3-May-18
///===================================================================================

function removeHtmlTagsForEmailCases() {
    try {
        if (Xrm.Page.ui.getFormType() != 1) {//Check if the form type is not create form
            var caseOrigin = Xrm.Page.getAttribute("caseorigincode").getValue();
            if (caseOrigin != null && (caseOrigin == 2 || caseOrigin == 4)) {
                var caseDescription = Xrm.Page.getAttribute("description").getValue();
                if (caseDescription != undefined || caseDescription != null || caseDescription != "") {
                    if (isHTML(caseDescription)) {

                        var txtDesc = stripHTML(caseDescription);

                        Xrm.Page.getAttribute("description").setValue(""); // Clear the field to avoid duplicate text on next onload

                        Xrm.Page.getAttribute("description").setValue(txtDesc); // Set the field with final email body

                        Xrm.Page.data.entity.save();// Save the form after removing the spaces in Description field.
                    }
                }
            }
        }
    } catch (e) {
        Xrm.Utility.alertDialog(arguments.callee.toString().match(/function\s+([^\s\(]+)/)[1].toString() + " >> " + e.message);
    }
}

function stripHTML(textToStrip) {

    var tmp = document.createElement("DIV");
    tmp.innerHTML = textToStrip;
    return tmp.textContent.trim() || tmp.innerText.trim();
}

function isHTML(str) {
    var a = document.createElement('div');
    a.innerHTML = str;

    for (var c = a.childNodes, i = c.length; i--;) {
        if (c[i].nodeType == 1) return true;
    }
    return false;
}

// JavaScript source code
///==================================================================================
/// Description: Function to set origin based on security role of logged in user
/// Created By: Mani
/// Event: Form Onload
/// Created On  : 7-5-18
///===================================================================================

function setOrigin() {
    debugger;
    var CCAUser = CheckUserRole("Call Center Agent");
    if (Xrm.Page.ui.getFormType() == 1) { //Check if the form type is create form
        if (CCAUser) {
            Xrm.Page.data.entity.attributes.get("caseorigincode").setValue(1);
        }
    }
}
function CheckUserRole(roleName) {
    var currentUserRoles = Xrm.Page.context.getUserRoles();
    for (var i = 0; i < currentUserRoles.length; i++) {
        var userRoleId = currentUserRoles[i];
        var userRoleName = GetRoleName(userRoleId);
        if (userRoleName == roleName) {
            return true;
        }
    }
    return false;
}
function GetRoleName(userRoleId) {
    if (typeof ($) === 'undefined') {
        $ = parent.$;
        jQuery = parent.jQuery;
    }
    var selectQuery = "RoleSet?$top=1&$filter=RoleId eq guid'" + userRoleId + "'&$select=Name";
    var odataSelect = GetServerUrl() + selectQuery;
    //alert(odataSelect);
    var roleName = null;
    $.ajax({
        type: "GET",
        async: false,
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        url: odataSelect,
        beforeSend: function (XMLHttpRequest) { XMLHttpRequest.setRequestHeader("Accept", "application/json"); },
        success: function (data, textStatus, XmlHttpRequest) {
            var result = data.d;
            if (!!result) {
                roleName = result.results[0].Name;
            }
        },
        error: function (XmlHttpRequest, textStatus, errorThrown) {
            //alert('OData Select Failed: ' + odataSelect);
        }
    });
    return roleName;
}
function GetServerUrl() {
    return Xrm.Page.context.getClientUrl() + "/xrmservices/2011/organizationdata.svc/";
}

///==================================================================================
/// Description : Show Consumer Affairs Section Button.
/// Created By  : Sameer
/// Event       : OnChange of Status Reason
/// Created On  : 09-May-18
///===================================================================================
function ShowConsumerASectionBtn() {
    try {
        if (Xrm.Page.getControl("header_statuscode").getAttribute() != null)
            var StatusCode = Xrm.Page.getControl("header_statuscode").getAttribute().getValue();
        if (StatusCode == 167490016) {
            Xrm.Page.ui.controls.get("tra_notificationstatus").setVisible(true);
            Xrm.Page.ui.controls.get("tra_satisfactionlevel").setVisible(true);
            //Xrm.Page.getAttribute("tra_assigntosp").setValue(null);
            Xrm.Page.data.entity.save();
        }
    }

    catch (err) {
        alert(' Show ConsumerASectionBtnfunction Error:' + err.description);
    }
}

///==================================================================================
/// Description : Function for pop up value on Accept Rejection Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 09-May-18
///===================================================================================
function SetAcceptRejection() {
    try {
        if (Xrm.Page.getAttribute("tra_acceptrejection") != null)
            var AcceptRejection = Xrm.Page.getAttribute("tra_acceptrejection").getValue();
        if (AcceptRejection == null || AcceptRejection == "") {

            Xrm.Utility.confirmDialog("This case will now be assigned to Call Center. Continue?", function () {
                Xrm.Page.getAttribute("tra_acceptrejection").setValue(1);
                //Xrm.Page.getAttribute("statuscode").setValue(167490019);
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_acceptrejection").setValue(null);

            });
        }
    }

    catch (err) {
        alert('Set Accept Rejection function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Function for pop up value on Reject Rejection Btn
/// Created By  : Sameer
/// Event       : Ribbon Btn
/// Created On  : 09-May-18
///===================================================================================
function SetRejectRejection() {
    try {
        var operatorslafailedvar = Xrm.Page.getAttribute("tra_operatorslafailed").getValue();
        if (operatorslafailedvar == 0) {
            Xrm.Page.getAttribute("tra_operatorslafailed").setValue(1);
        }
        if (Xrm.Page.getAttribute("tra_acceptrejection") != null)
            var AcceptRejection = Xrm.Page.getAttribute("tra_acceptrejection").getValue();
        if (AcceptRejection == null || AcceptRejection == "") {

            Xrm.Utility.confirmDialog("Confirm Rejection of Service Provider Decision?", function () {
                Xrm.Page.getAttribute("tra_acceptrejection").setValue(2);
                Xrm.Page.getAttribute("statuscode").setValue(167490025);
                Xrm.Page.data.entity.save();


            }, function () {

                Xrm.Page.getAttribute("tra_acceptrejection").setValue(null);

            });
        }
    }

    catch (err) {
        alert('Set Reject Rejection function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Provide Link in Email entity Form. 
/// Created By  : Sameer
/// Event       : Ribbon JS(Generate Doc.Link)
/// Created On  : 09-May-18
///===================================================================================
function GenerateDocLink() {
    try {
        var desciption = Xrm.Page.data.entity.attributes.get("description").getValue();
        var LinkUrl = "\n http://192.168.191.13:85/Portal/ConsumerPortal/en-US/MissingDocument.aspx?CaseId=";
        if (Xrm.Page.data.entity.attributes.get("regardingobjectid").getValue() != null) {

            //var CaseId = Xrm.Page.data.entity.attributes.get("regardingobjectid").getValue()[0].id;

            var CaseName = Xrm.Page.data.entity.attributes.get("regardingobjectid").getValue()[0].name;

            //var CaseType = Xrm.Page.data.entity.attributes.get("regardingobjectid ").getValue()[0].entityType;

        }
        //  var RegardingObjectId = Xrm.Page.getAttribute("regardingobjectid").getValue();
        Xrm.Page.data.entity.attributes.get("description").setValue(desciption + LinkUrl + CaseName);

    }

    catch (err) {
        alert('GenerateDocLink function Error:' + err.description);
    }
}


///==================================================================================
/// Description : Clear lookup value Complaint Subtype 
/// Created By  : Sameer
/// Event       : OnChange of Complaint type
/// Created On  : 22-May-18
///===================================================================================
function ComplaintType() {
    try {
        var _complaintsubtype = Xrm.Page.getAttribute("tra_complaintsubtype");

        if (_complaintsubtype != null) {

            Xrm.Page.getAttribute("tra_complaintsubtype").setValue(null);

        }
    }
    catch (err) {
        alert('ComplaintType function Error:' + err.description);
    }

}

///==================================================================================
/// Description : Clear lookup value on Service 
/// Created By  : Sameer
/// Event       : OnChange of Service Provider
/// Created On  : 22-May-18
///===================================================================================
function ServiceProvider() {
    try {
        var _service = Xrm.Page.getAttribute("tra_service");

        if (_service != null) {

            Xrm.Page.getAttribute("tra_service").setValue(null);

        }
    }
    catch (err) {
        alert('ServiceProvider function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Clear lookup value on Service
/// Created By  : Sameer
/// Event       : OnChange of Service
/// Created On  : 22-May-18
///===================================================================================
function Service() {
    try {
        var _servicesubtype = Xrm.Page.getAttribute("tra_servicesubtype");

        if (_servicesubtype != null) {

            Xrm.Page.getAttribute("tra_servicesubtype").setValue(null);

        }
    }
    catch (err) {
        alert('Service function Error:' + err.description);
    }
}

function HideSocialPaneItems() {

    var tabs = ["NOTES"];
    for (var tabsid = 0; tabsid < tabs.length; tabsid++) {
        HideTabs(tabs[tabsid]);
    }
}

function HideTabs(socialPaneType) {
    var ctrlElement = document.getElementById('header_notescontrol');
    if (ctrlElement == null) ctrlElement = window.parent.document.getElementById('header_notescontrol');
    if (ctrlElement == null) return;
    if (ctrlElement.children != null && ctrlElement.children.length > 0) {
        for (var ele = 0; ele < ctrlElement.children.length; ele++) {
            var ctrl = ctrlElement.children[ele];
            if (ctrl.title == socialPaneType) {
                ctrl.style.disabled = "disabled";
                if (ele + 1 < ctrlElement.children.length) { ctrlElement.children[ele + 1].click(); return; }
                else if (ele - 1 >= 0) {
                    ctrlElement.children[ele - 1].click();
                    return;
                }
            }
        }
    }
}

function hideconsumeraffairforenq() {
    try {

        var AssigntoConsumer = Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").getValue();
        var caseType = Xrm.Page.getAttribute("casetypecode").getValue();
        if (caseType == 2 && AssigntoConsumer == false) {
            return true;
        }

        else {
            return false;
        }
    }
    catch (err) {
        alert('Hide Assign to consumer N addl info reqd function Error:' + err.description);
    }
}
function onclickAssignToConAffairsEnq() {
    var casetype = Xrm.Page.getAttribute("casetypecode").getValue();
    var caseTypes;
    if (casetype == 2) {
        caseTypes = "enquiry";
    } else if (casetype == 3) {
        caseTypes = "suggestion";
    }
    Xrm.Utility.confirmDialog("Are you sure you want to assign this " + caseTypes + " to consumer affairs?", function () {
        var caseType = Xrm.Page.getAttribute("casetypecode").getValue();
        Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").setValue(true);
        Xrm.Page.getAttribute("tra_issuedecision").setValue(1);
        /// changes for Enquiry CR
        Xrm.Page.getAttribute("tra_assigntodepartment").setValue(1);
        if (caseType == 2 || caseType == 3) {
            //Xrm.Page.getAttribute("tra_cafirstassignment").setValue(true);
            Xrm.Page.getAttribute("statuscode").setValue(167490017);
        }


        Xrm.Page.data.entity.save();
    }, function () {

        return false;
    });
}

///==================================================================================
/// Description : Function for CA First Assignment (Accept Case button)
/// Created By  : Nikita Pawar
/// Event       : Ribbon Btn
/// Created On  : 10-May-19
///===================================================================================
function onAcceptCaseBtn() {
    try {
        var IsCAassigned = Xrm.Page.getAttribute("tra_cafirstassignment").getValue();
        if (IsCAassigned != null || IsCAassigned != "") {
            Xrm.Utility.confirmDialog("Please make sure that all required information is available. Would you like to continue?", function () {
                var currentDateTime = new Date();
                Xrm.Page.getAttribute("tra_caacceptedon").setValue(currentDateTime);
                Xrm.Page.getAttribute("tra_cafirstassignment").setValue(0);
                Xrm.Page.data.entity.save();
            }, function () {

                Xrm.Page.getAttribute("tra_caacceptedon").setValue(null);
                Xrm.Page.getAttribute("tra_cafirstassignment").setValue(1);
            });
        }
    }
    catch (err) {
        alert('onAcceptCaseBtn function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Show Accept Case
/// Created By  : Nikita Pawar
/// Event       : Ribbon Btn
/// Created On  : 10-May-19
///===================================================================================
function showAcceptcase() {
    try {
        var isCAassigned = Xrm.Page.getAttribute("tra_cafirstassignment").getValue();
        var caseTypeCode = Xrm.Page.getAttribute("casetypecode").getValue();
        var docVerified = Xrm.Page.getAttribute("tra_documentsverified").getValue();
        var scopeAccepted = Xrm.Page.getAttribute("tra_scopeacceptance").getValue();
        var CAflagenquiry = Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").getValue();
        var caseacceptedon = Xrm.Page.getAttribute("tra_caacceptedon").getValue();

        if ((isCAassigned == 1 && caseTypeCode == 1 && docVerified == 2 && scopeAccepted == 1)
            || ((caseTypeCode == 2 || caseTypeCode == 3) && isCAassigned == 1 && CAflagenquiry == 1 && (caseacceptedon == null || caseacceptedon == ""))) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('showAcceptcase function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Show Assign to Department,Process case,Assign to operator buttons
/// Created By  : Nikita Pawar
/// Event       : Ribbon Btn
/// Created On  : 10-May-19
///===================================================================================
function ShowAfterfirstCAassignment() {
    try {
        var isCAassigned = Xrm.Page.getAttribute("tra_cafirstassignment").getValue();
        var caseTypeCode = Xrm.Page.getAttribute("casetypecode").getValue();

        if (isCAassigned == 0) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('ShowAfterCAassignment function Error:' + err.description);
    }
}



///==================================================================================
/// Description : Show hide feedback for ENquiry
/// Created By  : Nikita Pawar
/// Event       : Ribbon Btn
/// Created On  : 10-May-19
///===================================================================================
function ShowhideFeedbackforENq() {
    try {
        var statuscode = Xrm.Page.getAttribute("statuscode").getValue();
        var cAssignmet = Xrm.Page.getAttribute("tra_caacceptedon").getValue();

        if ((statuscode == 167490017 || statuscode == 167490001) && cAssignmet != null) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('ShowhideFeedbackforENq function Error:' + err.description);
    }
}


///==================================================================================
/// Description : On click: AssignToDept_En: Assign case to selected department
/// Created By  : Vishal
/// Event       : Ribbon Btn
/// Created On  : 10-May-19
///===================================================================================
function OnClick_AssignToDept_En() {
    try {
        if (Xrm.Page.getAttribute("tra_department") != null)
            var Department = Xrm.Page.getAttribute("tra_department").getValue();
        //var name = Xrm.Page.getAttribute("tra_department").getValue()[0].name;
        if (Department == null) {
            Xrm.Utility.confirmDialog("Please select a department!", function () {
                Xrm.Page.ui.controls.get("tra_department").setFocus();
            },
                function () {
                    Xrm.Page.ui.controls.get("tra_department").setFocus();


                });
        }
        if (Department != null) {
            var name = Xrm.Page.getAttribute("tra_department").getValue()[0].name;
            var casetype = Xrm.Page.getAttribute("casetypecode").getValue();
            var caseTypes;
            if (casetype == 2) {
                caseTypes = "enquiry";
            } else if (casetype == 3) {
                caseTypes = "suggestion";
            }

            Xrm.Utility.confirmDialog("This " + caseTypes + " will now be assigned to '" + name + "' department. Continue?", function () {
                debugger;
                Xrm.Page.getAttribute("tra_assigntodepartment").setValue(2);
                Xrm.Page.getAttribute("statuscode").setValue(167490018);
                Xrm.Page.getAttribute("tra_assigntoconsumeraffairs").setValue();
                /// Changes for Enquiry CR
                Xrm.Page.getAttribute("tra_consumeraffairsflagenquiry").setValue(0);


                Xrm.Page.data.entity.save();
            }, function () {

                // Xrm.Page.getAttribute("tra_department").setValue(null);

            });
        }
    }
    catch (err) {
        alert('Set Department function Error:' + err.description);
    }
}



///==================================================================================
/// Description: Function for hide archived fields for non archive cases
/// Created By: Mani
/// Event: Ribbon button
/// Created On  : 03-06-2020
///===================================================================================

function hideArchiveSection() {
    var isArchive = Xrm.Page.getAttribute("tra_isarchive").getValue();
    if (isArchive == true) {
        Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Tab_Summary_section_8").setVisible(true);
    }
    else {
        Xrm.Page.ui.tabs.get("Tab_Summary").sections.get("Tab_Summary_section_8").setVisible(false);
    }
}

///==================================================================================
/// Description: Function on click of Process Enquiry button
/// Created By: Vishal
/// Event: Ribbon button
/// Created On  : 12-06-2020
///===================================================================================

function OnClick_ProcessEnquiry_En() {
    try {
        var status = Xrm.Page.getAttribute("statuscode").getValue();
        var casetype = Xrm.Page.getAttribute("casetypecode").getValue();

        if (status == 167490017 && casetype == 2) {
            Xrm.Page.getAttribute("statuscode").setValue(167490001);
            Xrm.Page.data.entity.save();
        }
    }
    catch (err) {
        alert('OnClick_ProcessEnquiry_En function Error:' + err.description);
    }
}

///==================================================================================
/// Description : Show Process Enquiry button
/// Created By  : Vishal Yadav
/// Event       : Ribbon Btn
/// Created On  : 12-06-2020
///===================================================================================
function ShowHideProcessEnquiry_En() {
    try {
        var isCAassigned = Xrm.Page.getAttribute("tra_cafirstassignment").getValue();
        var caseTypeCode = Xrm.Page.getAttribute("casetypecode").getValue();

        if (isCAassigned == 0 && caseTypeCode == 2) {
            return true;
        }
        else {
            return false;
        }
    }
    catch (err) {
        alert('ShowAfterCAassignment function Error:' + err.description);
    }
}
