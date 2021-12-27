using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using TRAWebApplication.Authorization;
using TRAWebApplication.BuisnessLayer;
using TRAWebApplication.ConfigSetting;
using TRAWebApplication.Models;

namespace TRAWebApplication.Controllers
{
    public class EnglishConsumerController : ApiController
    {

        IEnglishConsumer _IConsumer = new EnglishConsumer();

        [Authorize]
        [HttpGet]
        [Route("api/EnglishConsumer/getSignedConsumer")]
        public async Task<IHttpActionResult> getUserInfo()
        {

            AccountModel _accountData = new AccountModel();

            try
            {
                var  identityClaims = (ClaimsIdentity)User.Identity;

                IEnumerable<Claim> claims = identityClaims.Claims;

                _accountData.fullname = identityClaims.FindFirst("fullname").Value;
                _accountData.contactid = identityClaims.FindFirst("contactId").Value;
                _accountData.loggedTime = identityClaims.FindFirst("Loggedon").Value;
                _accountData.errorcode = identityClaims.FindFirst("errorCode").Value;
                _accountData.firstname = identityClaims.FindFirst("firstname").Value;

            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }

            return Ok(_accountData);
        }

        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getServiceProviders")]
        public async Task<IHttpActionResult> getServiceProviders()
        {
            try
            {
                var serviceproviders = await _IConsumer.getSrvcProviders();
                return Ok(serviceproviders);
            }
            catch (Exception ex)
            {

                throw ex; 
            }


        }


        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getServices")]
        public async Task<IHttpActionResult> services([FromUri] string id)
        {
            try
            {
                var services = await _IConsumer.getService(id);
                return Ok(services);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [HttpGet]     
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getComplaints")]
        public async Task<IHttpActionResult> getComplaints()
        {

            try
            {
                var complaintsData = await _IConsumer.complaints();
                return Ok(complaintsData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getComplaintSubTypes")]
        public async Task<IHttpActionResult> getComplaintSubType([FromUri]string complaintId)
        {
            try
            {

                var complaintsSubData = await _IConsumer.getComplaintSubTypes(complaintId);
                return Ok(complaintsSubData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize]      
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("api/EnglishConsumer/createEnquiry")]
        public async Task<IHttpActionResult> createEnquiry()
        {

            try
            {

                var enquiry = await _IConsumer.createEnquiry();
                return Ok(enquiry);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/createSuggestion")]
        public async Task<IHttpActionResult> createSuggestion()
        {
            try
            {

                var suggestions = await _IConsumer.createSuggestion();
                return Ok(suggestions);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]       
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/createComplaints")]
        public async Task<IHttpActionResult> createComplaints()
        {
            try
            {

                var complaints = await _IConsumer.Addcomplaints();
                return Ok(complaints);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]       
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getCasebyContact")]
        public async Task<IHttpActionResult> getCasebyContact([FromUri]string contactid)
        {
            try
            {

                var cases = await _IConsumer.mycases(contactid);
                return Ok(cases);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getCasedetails")]
        public async Task<IHttpActionResult> getCasedetails([FromUri] string caseno)
        {
            try
            {

                var caseInfo = await _IConsumer.caseDetails(caseno);
                return Ok(caseInfo);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getDocumentTypes")]
        public async Task<IHttpActionResult> getDocumentTypes([FromUri] string complaintsId)
        {
            try
            {

                var document = await _IConsumer.documentType(complaintsId);
                return Ok(document);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getEnquiryTypes")]
        public async Task<IHttpActionResult> getEnquiryTypes()
        {
            try
            {

                var enquiries = await _IConsumer.getEnquiry();
                return Ok(enquiries);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [HttpGet]   
        [Route("api/EnglishConsumer/downloadFiles")]
        public async Task<IHttpActionResult> downloadFiles([FromUri]string fileId)
        {
            

            try
            {

                var annotationFiles = await _IConsumer.downloadAttachments(fileId);
                
                return Ok(annotationFiles);


            }
            catch (Exception ex)
            {
                throw ex;

            }

        

        }
        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getnations")]
        public async Task<IHttpActionResult> getnations()
        {
            try
            {

                var nation = await _IConsumer.getNations();
                return Ok(nation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpGet]
        [Route("api/EnglishConsumer/getnationality")]
        public async Task<IHttpActionResult> getnationality()
        {
            try
            {

                var nation = await _IConsumer.getNations();
                return Ok(nation);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpGet]
        [Route("api/EnglishConsumer/getTitleInfo")]
        public async Task<IHttpActionResult> getTitleInfo()
        {
            try
            {

                var title = await _IConsumer.getTitle();
                return Ok(title);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [BasicAuth]
        [HttpGet]
        [Route("api/EnglishConsumer/getArea")]
        public async Task<IHttpActionResult> getArea()
        {
            try
            {

                var title = await _IConsumer.getArea();
                return Ok(title);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpPost]     
        [Route("api/EnglishConsumer/createIndivisualUser")]
        public async Task<IHttpActionResult> createIndivisualUser(TraUser indivisualUserObj)
        {
            try
            {

                var indivisualUser = await _IConsumer.indivisualUser(indivisualUserObj);
                return Ok(indivisualUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpPost]
        [Route("api/EnglishConsumer/createBuisnessUser")]
        public async Task<IHttpActionResult> createBuisnessUser(TraUser buisnessUser)
        {
            try
            {

                var buisnessUsers = await _IConsumer.AddBuisnessUser(buisnessUser);
                return Ok(buisnessUsers);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [BasicAuth]
        [HttpPost]
        [Route("api/EnglishConsumer/creatgovernmentEntityUser")]
        public async Task<IHttpActionResult> creatgovernmentEntityUser(TraUser governmentUser)
        {
            try
            {

                var governmentUsers = await _IConsumer.AddgovernmentUser(governmentUser);
                return Ok(governmentUsers);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpPost]       
        [Route("api/EnglishConsumer/checkEmailIsExist")]
        public async Task<IHttpActionResult> checkEmailIsExist(string emailId)
        {
            try
            {

                var isExist = await _IConsumer.IsEmailExist(emailId);
                return Ok(isExist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/checkPass")]
        public async Task<IHttpActionResult> checkPass(string password,string contactid)
        {
            try
            {

                var isExist = await _IConsumer.IsPassCheck(password, contactid);
                return Ok(isExist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [BasicAuth]
        [HttpPost]
        [Route("api/EnglishConsumer/checkCPRIsExist")]
        public async Task<IHttpActionResult> checkCPRIsExist(string cprno)
        {
            try
            {

                var isExist = await _IConsumer.IsCPRExist(cprno);
                return Ok(isExist);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
        


        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/getConsumerById")]
        public async Task<IHttpActionResult> getConsumerById([FromUri]string contactid)
        {
            try
            {

                var consumer = await _IConsumer.getConsumerbyId(contactid);
                return Ok(consumer);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/updateUserbyId")]
        public async Task<IHttpActionResult> updateUserbyId(TraUser _updateUser)
        {
            try
            {

                var consumerUpdates = await _IConsumer.updateUser(_updateUser);
                return Ok(consumerUpdates);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/EnglishConsumer/changeUserPassword")]
        public async Task<IHttpActionResult> changeUserPassword(changepasswordVM changepassword)
        {
            try
            {

                var consumerpasswordUpdates = await _IConsumer.passwordChangeUser(changepassword);
                return Ok(consumerpasswordUpdates);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpPost]
     
        [Route("api/EnglishConsumer/postDocumentRequest")]
        public async Task<IHttpActionResult> replyComment()
        {
            try
            {

                var replyConsumerComments = await _IConsumer.userComment();
                return Ok(replyConsumerComments);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpGet]      
        [Route("api/EnglishConsumer/getSuerveyInfo")]
        public async Task<IHttpActionResult> getSuerveyInfo([FromUri] string caseId)
        {
            try
            {

                var surveyInfo = await _IConsumer.surveyDetails(caseId);
                return Ok(surveyInfo);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [BasicAuth]
        [HttpGet]
        [Route("api/EnglishConsumer/getFixedprobs")]
        public async Task<IHttpActionResult> getFixedprobs([FromUri] string caseId)
        {
            try
            {

                var fixedProbs = await _IConsumer.getFixedProbs(caseId);
                return Ok(fixedProbs);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [BasicAuth]
        [HttpPost]
        [Route("api/EnglishConsumer/postConsumerSurvey")]
        public async Task<IHttpActionResult> postConsumerSurvey(surveyData _servey)
        {
            try
            {

                var surveyData = await _IConsumer.postSurvey(_servey);
                return Ok(surveyData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpPost]
        [Route("api/EnglishConsumer/postfixedProbsData")]
        public async Task<IHttpActionResult> postfixedProbsData(fixedProbsVM fixedprobsVM)
        {
            try
            {

                var fixedProbs = await _IConsumer.postfixedProbs(fixedprobsVM);
                return Ok(fixedProbs);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpGet]
        [Route("api/EnglishConsumer/getConsumerConsent")]
        public async Task<IHttpActionResult> getConsumerConsent([FromUri] string caseId)
        {
            try
            {

                var consumerConsent = await _IConsumer.getConsumerConsentInfo(caseId);
                return Ok(consumerConsent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      
        [BasicAuth]
        [HttpGet]
        [Route("api/EnglishConsumer/CancelConsent")]
        public async Task<IHttpActionResult> CancelConsent([FromUri] string caseId)
        {
            try
            {

                var consumerConsent = await _IConsumer.cancelConsent(caseId);
                return Ok(consumerConsent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpGet]
        [Route("api/EnglishConsumer/downloadConsentFiles")]
        public async Task<IHttpActionResult> downloadConsentFiles([FromUri]string fileId)
        {

            try
            {

                var annotationFiles = await _IConsumer.downloadAttachments(fileId);

                return Ok(annotationFiles);


            }
            catch (Exception ex)
            {
                throw ex;

            }



        }
        [BasicAuth]
        [HttpPost]
        [Route("api/EnglishConsumer/postConsumerConsentData")]
        public async Task<IHttpActionResult> postConsumerConsentData([FromBody]consumerConsent consentData)
        {

            try
            {

                var consentPostData = await _IConsumer.postConsentData(consentData);

                return Ok(consentPostData);


            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        [BasicAuth]
        [HttpGet]
        [Route("api/EnglishConsumer/getDocumentRequest")]
        public async Task<IHttpActionResult> getDocumentRequest([FromUri] string caseId)
        {
            try
            {

                var consumerConsent = await _IConsumer.getDocumentRequest(caseId);
                return Ok(consumerConsent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpPost]
        [Route("api/EnglishConsumer/forgotpassword")]
        public async Task<IHttpActionResult> forgotpassword([FromUri]string email)
        {
            try
            {

                var forgotPasswordUser = await _IConsumer.forGotPassword(email);
                return Ok(forgotPasswordUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        

    }
}

