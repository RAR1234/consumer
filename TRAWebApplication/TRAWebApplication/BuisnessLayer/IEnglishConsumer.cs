using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRAWebApplication.Models;

namespace TRAWebApplication.BuisnessLayer
{
    public interface IEnglishConsumer
    {
        Task<List<getServiceProviders>> getSrvcProviders();
        Task<List<servcClass>> getService(string id);

        Task<List<getComplaint>> complaints();
        Task<List<getComplaintSubType>> getComplaintSubTypes(string complaintid);

        Task<caseDetail> createEnquiry();

        Task<caseDetail> createSuggestion();

        Task<caseDetail> Addcomplaints();

        Task<List<myCases>> mycases(string contactid);

        Task<caseInformation> caseDetails(string caseno);

        Task<List<documentType>> documentType(string complaintid);

        Task<List<enquiryType>> getEnquiry();

        Task<annotationClass> downloadAttachments(string annotationid);

        Task<List<GetNationality>> getNations();

        Task<List<GetNationality>> getArea();

        Task<List<getTitle>> getTitle();

        Task<consumerSuccess> indivisualUser(TraUser user);

       


        Task<consumerSuccess> AddBuisnessUser(TraUser buisnessuser);

        Task<consumerSuccess> AddgovernmentUser(TraUser governmentuser);

        Task<Boolean> IsEmailExist(string email);
        Task<Boolean> IsPassCheck(string password,string contactid);

        Task<contactvm> getConsumerbyId(string contactid);

        Task<consumerSuccess> updateUser(TraUser _updateuser);

        Task<consumerSuccess> passwordChangeUser(changepasswordVM changepassword);

        Task<caseDetail> userComment();
        Task<Boolean> IsCPRExist(string cprno);
       Task<surveyDetails> surveyDetails(string caseid);

        Task<surveyDetails> postSurvey(surveyData survey);

        Task<caseInformation> getConsumerConsentInfo(string caseid);
        Task<caseInformation> getFixedProbs(string caseid);

        Task<caseInformation> getDocumentRequest(string caseid);

        Task<Boolean> forGotPassword(string email);
        Task<consumerSuccess> postfixedProbs(fixedProbsVM _fixedprobs);
        Task<consumerSuccess> postConsentData(consumerConsent _consentdata);
        Task<consumerSuccess> cancelConsent(string caseid);

    }
}
