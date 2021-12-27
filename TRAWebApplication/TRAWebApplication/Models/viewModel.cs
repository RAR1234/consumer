using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;

namespace TRAWebApplication.Models
{
    public class viewModel
    {
    }
    public class AccountModel
    {

        public string fullname { get; set; }
        public string contactid { get; set; }

        public string loggedTime { get; set; }
        public string errorcode { get; set; }
        public string firstname { get; set; }
    }
    public class serviceProviderDetails
    {


        public List<getServiceProviders> ServiceProviders { get; set; }
    }
    public class getServiceProviders
    {
        public string label { get; set; }
        public Guid value { get; set; }
    }

    public class contactvm
    {
        public string firstname { get; set; }
        public string lastname { get; set; }

        public string emailaddress { get; set; }
        public string countrycode { get; set; }
        public string createdon { get; set; }
        public string nationality { get; set; }
        public string contactno { get; set; }
        public string id_type { get; set; }
        public string idnumber { get; set; }
        public string fullnm { get; set; }
        public Guid nationalityId { get; set; }
        public Guid contactid { get; set; }
    }


    public class fixedProbsVM
    {
        
        public string flat { get; set; }
        public string buildingno { get; set; }

        public string roadno { get; set; }
        public string blockno { get; set; }
        public Guid area { get; set; }
        public Guid contactid { get; set; }
        public Guid serviceProviderId { get; set; }
        public string fullname { get; set; }
        public string ticketno { get; set; }

        public string packageName { get; set; }

        public string accessNetworkTech { get; set; }

        public string downloadThreshold { get; set; }

        public string downloadspeed { get; set; }

        public string uploadSpeed { get; set; }

        public string throttlespdDwnload { get; set; }
        public string throttlespdupload { get; set; }
        public string signature { get; set; }
        public Guid caseId { get; set; }

    }
    public class caseDetails
    {
        public Guid caseId { get; set; }
        public string caseNo { get; set; }
        public string caseType { get; set; }

        public string status { get; set; }
        public string createOn { get; set; }
        public string stateCode { get; set; }

        public DateTime submitComplaintDate { get; set; }

        public string service { get; set; }

        public string serviceProvider { get; set; }

        public string caseRefNo { get; set; }

        public DateTime? complaintAgainstSp { get; set; }

        public string caseDescription { get; set; }
       
        public string complaintType { get; set; }
        public string fullname { get; set; }

        public string block { get; set; }
        public string flat { get; set; }
        public string building { get; set; }
        public string road { get; set; }

        public string label { get; set; }
        public Guid value { get; set; }
        public string address { get; set; }
        public string satisfySolution { get; set; }
        public string actionTakeSp { get; set; }
        public Guid serviceId { get; set; }
        public Guid serviceProviderId { get; set; }
        public Guid complaintId { get; set; }
        public Guid contactid { get; set; }

        public string disputNo { get; set; }
    }
    public class conversationView
    {
        public Guid conversationId { get; set; }
        public string comments { get; set; }
        public string commentBy { get; set; }
        public Boolean isPortal { get; set; }
        public DateTime createdOn { get; set; }
        public Boolean IsConsumer { get; set; }
    }
    public class srvcProvider
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
    public class servcClass
    {

        public string label { get; set; }
        public Guid value { get; set; }
    }

    public class enquiryType
    {

        public string label { get; set; }
        public Guid value { get; set; }
    }

    public class caseInformation
    {

        public List<annotationClass> listAnnotation { get; set; }
        public caseDetails _caseData { get; set; }
        public Boolean _IsConsent { get; set; }

        public Boolean IsFixedProbs { get; set; }
        public List<conversationView> CommentbyListFromConsumer { get; set; }
        public List<conversationView> CommentbyListFromTRA { get; set; }

        public List<conversationView> CommentbyListFromOperator { get; set; }


    }
    public class replyModel
    {
        public Guid caseId { get; set; }
        public string caseNo { get; set; }
        public string replyComment { get; set; }
    }
    public class GetNationality
    {
        public string label { get; set; }
        public Guid value { get; set; }
    }

    public class getTitle
    {
        public string label { get; set; }
        public Guid value { get; set; }
    }
    public class annotationClass
    {
        public Guid annotId { get; set; }
        public string documentbody { get; set; }
        public string filename { get; set; }
        public string mimeType { get; set; }
        public bool IsportalAttach { get; set; }

        public bool isConsumer { get; set; }

       public string commentBy { get; set; }
        
    }
    public class googleUser
    {
        public string getId { get; set; }
        public string getName { get; set; }
        public string getEmail { get; set; }

    }
    public class consumerConsent
    {
        public string question1 { get; set; }
        public string question2 { get; set; }
        public string question3 { get; set; }
        public Guid contactid { get; set; }
        public Guid  serviceId  {get;set;}
        public Guid complaintId { get; set; }
        public Guid serviceProviderId { get; set; }
        public string ticketno { get; set; }
        public DateTime dateSubmit { get; set; }

        public string caseType { get; set; }

        public string disputeNo { get; set; }

        public string caseRefNo { get; set; }

        public string complaintDate { get; set; }
        public Guid caseId { get; set; }


    }
    public class getComplaintSubType
    {
        public string label { get; set; }
        public Guid value { get; set; }
    }
    public class getComplaint
    {
        public string label { get; set; }
        public Guid value { get; set; }
    }
    public class myCases
    {
        public string case_refNo { get; set; }
        public string date_submitted { get; set; }
        public string case_type { get; set; }
        public string case_status { get; set; }
        public Guid caseId { get; set; }

        public DateTime createdDate { get; set; }

        //public int srNo { get; set; }


    }
    public class facebookUsers
    {
        public fbuser data1 { get; set; }

        public string fbToken { get; set; }
    }

    public class fbuser
    {
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
    }
    public class TraUser
    {

        public string socialProviderKey { get; set; }
        public string socialProvider { get; set; }
        public string fullname { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string nationality { get; set; }
        public string IdType { get; set; }
        public string CprPassportNo { get; set; }
        public string emailId { get; set; }
        public DateTime dateOfbirth { get; set; }
        public string countryCode { get; set; }
        public string contact { get; set; }
        public string password { get; set; }

        public string DOB { get; set; }

        public string companyName { get; set; }
        public string commercialNo { get; set; }

        public string governmentEntity { get; set; }
        public Guid titleId { get; set; }
        public Guid nationId { get; set; }
        public Guid ContactId { get; set; }

    }
    public class loginUser
    {
        public string emailId { get; set; }
        public string password { get; set; }
    }


    public class caseDetail
    {
        public HttpStatusCode errorCode { get; set; }
        public string message { get; set; }
        public string caseNo { get; set; }
        public Guid caseId { get; set; }
    }

    public class consumerSuccess
    {
        public HttpStatusCode errorCode { get; set; }
        public string message { get; set; }

        public string fullname { get; set; }
        public string firstname { get; set; }
        public Guid contactId { get; set; }

        public ClaimsIdentity Identity { get; set; }

    }
    public class documentType
    {
        public string document { get; set; }
        public Guid documentId { get; set; }
    }

    public class consumerLogin
    {
        public string emailId { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public Guid contactId { get; set; }
        public string firstname { get; set; }
    }
    public class ComplaintObject
    {
        public string voice { get; set; }
        public string data { get; set; }
        public string complaintSubType { get; set; }
        public string complaintType { get; set; }
        public string srcvProvider { get; set; }
        public string services { get; set; }
        public string contactId { get; set; }
        public string flat { get; set; }
        public string buildingNo { get; set; }
        public string roadStreet { get; set; }
        public string blockNo { get; set; }
       

        public string complaintDescription { get; set; }
        public string spResponse { get; set; }
        public string solutionSatisfy { get; set; }
        public string disputeNo { get; set; }
        public string subscriptionType { get; set; }
        public string complaintDate { get; set; }
        public string caseRefNo { get; set; }

        public string tra_ownerofdisputednumber { get; set; }
        public string ownerNm { get; set; }

        public string cprNoofowner { get; set; }
        public string complaintName { get; set; }
        public string disputedAmount { get; set; }
        public string typepackage { get; set; }

    }
    public class postedFile
    {
        public HttpPostedFile httppostedFile { get; set; }
        public string filename { get; set; }

        public string documntsName { get; set; }
    }

    public class documentBody
    {
        public string documentbody { get; set; }
        public string filename { get; set; }

        public string documentType { get; set; }
    }
    public class documentsData
    {
        public string documents { get; set; }
    }

    public class changepasswordVM
    {
        public Guid contactid { get; set; }
        public string currentPassword { get; set; }
        public string newPassword { get; set; }

    }
    public class surveyDetails
    {
        public Boolean isSurvey { get; set; }
        public string ticketNo { get; set; }

        public HttpStatusCode errorCode { get; set; }
    }

    public class surveyData
    {
        public string survey1 { get; set; }
        public string survey2 { get; set; }
        public string survey3 { get; set; }
        public string survey4 { get; set; }
        public string comments { get; set; }
        public Guid caseid { get; set; }
    }
}
