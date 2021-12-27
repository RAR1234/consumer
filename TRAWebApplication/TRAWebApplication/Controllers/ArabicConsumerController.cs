using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using TRAWebApplication.Authorization;
using TRAWebApplication.BuisnessLayer;
using TRAWebApplication.Models;

namespace TRAWebApplication.Controllers
{
    public class ArabicConsumerController : ApiController
    {

        IArabicConsumer _arabicConsumer = new ArabicConsumer();


        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/ArabicConsumer/getServiceProvidersAr")]
        public async Task<IHttpActionResult> getServiceProviders()
        {
            try
            {
                var serviceprovidersAr = await _arabicConsumer.getSrvcProviders();
                return Ok(serviceprovidersAr);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/ArabicConsumer/getServicesAr")]
        public async Task<IHttpActionResult> services([FromUri] string id)
        {
            try
            {
                var services = await _arabicConsumer.getService(id);
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
        [Route("api/ArabicConsumer/getComplaintsAr")]
        public async Task<IHttpActionResult> getComplaints()
        {

            try
            {
                var complaintsDataAr = await _arabicConsumer.complaints();
                return Ok(complaintsDataAr);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/ArabicConsumer/getComplaintSubTypesAr")]
        public async Task<IHttpActionResult> getComplaintSubType([FromUri]string complaintId)
        {
            try
            {

                var complaintsSubDataAr = await _arabicConsumer.getComplaintSubTypes(complaintId);
                return Ok(complaintsSubDataAr);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/ArabicConsumer/getDocumentTypes")]
        public async Task<IHttpActionResult> getDocumentTypes([FromUri] string complaintsId)
        {
            try
            {

                var document = await _arabicConsumer.documentType(complaintsId);
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
        [Route("api/ArabicConsumer/getEnquiryTypesAr")]
        public async Task<IHttpActionResult> getEnquiryTypes()
        {
            try
            {

                var enquiries = await _arabicConsumer.getEnquiry();
                return Ok(enquiries);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/ArabicConsumer/createComplaintsAr")]
        public async Task<IHttpActionResult> createComplaints()
        {
            try
            {

                var complaints = await _arabicConsumer.AddcomplaintsAr();
                return Ok(complaints);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpPost]
        [Route("api/ArabicConsumer/createEnquiryAr")]
        public async Task<IHttpActionResult> createEnquiry()
        {

            try
            {

                var enquiry = await _arabicConsumer.createEnquiry();
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
        [Route("api/ArabicConsumer/createSuggestionAr")]
        public async Task<IHttpActionResult> createSuggestion()
        {
            try
            {

                var suggestions = await _arabicConsumer.createSuggestion();
                return Ok(suggestions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/ArabicConsumer/getCasebyContactAr")]
        public async Task<IHttpActionResult> getCasebyContact([FromUri]string contactid)
        {
            try
            {

                var cases = await _arabicConsumer.mycases(contactid);
                return Ok(cases);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [HttpGet]
        [Route("api/ArabicConsumer/getCasedetailsAr")]
        public async Task<IHttpActionResult> getCasedetails([FromUri] string caseno)
        {
            try
            {

                var caseInfo = await _arabicConsumer.caseDetails(caseno);
                return Ok(caseInfo);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("api/ArabicConsumer/downloadFilesAr")]
        public async Task<IHttpActionResult> downloadFiles([FromUri]string fileId)
        {
          

            try
            {

                var annotationFiles = await _arabicConsumer.downloadAttachments(fileId);

                if (annotationFiles != null)
                {

                    return Ok(annotationFiles);
       
                }

            }
            catch (Exception ex)
            {
                throw ex;
              
            }

            return Ok();

        }

        [BasicAuth]
        [HttpGet]
        [Route("api/ArabicConsumer/getnationalityAr")]
        public async Task<IHttpActionResult> getnationality()
        {
            try
            {

                var nationAr = await _arabicConsumer.getNationsAr();
                return Ok(nationAr);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [Authorize]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [HttpGet]
        [Route("api/ArabicConsumer/getnationAr")]
        public async Task<IHttpActionResult> getnation()
        {
            try
            {

                var nationsAr = await _arabicConsumer.getNationsAr();
                return Ok(nationsAr);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/ArabicConsumer/getConsumerArById")]
        public async Task<IHttpActionResult> getConsumerById([FromUri]string contactid)
        {
            try
            {

                var consumer = await _arabicConsumer.getConsumerbyId(contactid);
                return Ok(consumer);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [HttpPost]
        [Route("api/ArabicConsumer/updateUserArbyId")]
        public async Task<IHttpActionResult> updateUserbyId(TraUser _updateUser)
        {
            try
            {

                var consumerUpdates = await _arabicConsumer.updateUserAr(_updateUser);
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
        [Route("api/ArabicConsumer/changeArUserPassword")]
        public async Task<IHttpActionResult> changeUserPassword(changepasswordVM changepassword)
        {
            try
            {

                var ArconsumerpasswordUpdates = await _arabicConsumer.passwordChangeUser(changepassword);
                return Ok(ArconsumerpasswordUpdates);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [BasicAuth]
        [HttpPost]
        [Route("api/ArabicConsumer/createIndivisualArUser")]
        public async Task<IHttpActionResult> createIndivisualUser(TraUser indivisualUserObj)
        {
            try
            {

                var indivisualUser = await _arabicConsumer.indivisualArUser(indivisualUserObj);
                return Ok(indivisualUser);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [BasicAuth]
        [HttpPost]
        [Route("api/ArabicConsumer/createArBuisnessUser")]
        public async Task<IHttpActionResult> createBuisnessUser(TraUser buisnessUser)
        {
            try
            {

                var buisnessUsers = await _arabicConsumer.AddBuisnessUser(buisnessUser);
                return Ok(buisnessUsers);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [BasicAuth]
        [HttpPost]
        [Route("api/ArabicConsumer/creatgovernmentEntityArUser")]
        public async Task<IHttpActionResult> creatgovernmentEntityUser(TraUser governmentUser)
        {
            try
            {

                var governmentUsers = await _arabicConsumer.AddgovernmentUser(governmentUser);
                return Ok(governmentUsers);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Authorize]
        [Route("api/ArabicConsumer/createReplyCommentsAr")]
        public async Task<IHttpActionResult> replyComment()
        {
            try
            {

                var replyConsumerCommentsAr = await _arabicConsumer.userCommentAr();
                return Ok(replyConsumerCommentsAr);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [BasicAuth]
        [HttpGet]
        [Route("api/ArabicConsumer/getConsumerConsentAr")]
        public async Task<IHttpActionResult> getConsumerConsent([FromUri] string caseId)
        {
            try
            {

                var consumerConsent = await _arabicConsumer.getConsumerConsentInfo(caseId);
                return Ok(consumerConsent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpPost]
        [Route("api/ArabicConsumer/postConsumerConsentDataAr")]
        public async Task<IHttpActionResult> postConsumerConsentData([FromBody]consumerConsent consentData)
        {

            try
            {

                var consentPostData = await _arabicConsumer.postConsentData(consentData);

                return Ok(consentPostData);


            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        [BasicAuth]
        [HttpGet]
        [Route("api/ArabicConsumer/getDocumentRequestAr")]
        public async Task<IHttpActionResult> getDocumentRequest([FromUri] string caseId)
        {
            try
            {

                var consumerConsent = await _arabicConsumer.getDocumentRequest(caseId);
                return Ok(consumerConsent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuth]
        [HttpGet]
        [Route("api/ArabicConsumer/getTitleInfo")]
        public async Task<IHttpActionResult> getTitleInfo()
        {
            try
            {

                var title = await _arabicConsumer.getTitle();
                return Ok(title);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
