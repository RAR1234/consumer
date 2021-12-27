
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRAWebApplication.Models;

namespace TRAWebApplication.BuisnessLayer
{
  public  interface IArabicConsumer
    {
        Task<caseDetail> userCommentAr();

        Task<List<getTitle>> getTitle();
        Task<List<getServiceProviders>> getSrvcProviders();
        Task<List<getComplaint>> complaints();

        Task<List<servcClass>> getService(string id);

        Task<caseInformation> getDocumentRequest(string caseid);
        Task<List<getComplaintSubType>> getComplaintSubTypes(string complaintid);
        Task<List<documentType>> documentType(string complaintid);

        Task<caseDetail> AddcomplaintsAr();
        Task<caseDetail> createSuggestion();
        Task<List<myCases>> mycases(string contactid);

        Task<List<enquiryType>> getEnquiry();
        Task<caseDetail> createEnquiry();

        Task<caseInformation> caseDetails(string caseno);
        Task<annotationClass> downloadAttachments(string annotationid);

        Task<List<GetNationality>> getNationsAr();
        Task<contactvm> getConsumerbyId(string contactid);
        Task<consumerSuccess> updateUserAr(TraUser _updateuser);

        Task<consumerSuccess> passwordChangeUser(changepasswordVM changepassword);

        Task<consumerSuccess> indivisualArUser(TraUser user);
        Task<consumerSuccess> AddgovernmentUser(TraUser governmentuser);

        Task<consumerSuccess> AddBuisnessUser(TraUser buisnessuser);

        Task<caseInformation> getConsumerConsentInfo(string caseid);

        Task<consumerSuccess> postConsentData(consumerConsent _consentdata);
    }
}
