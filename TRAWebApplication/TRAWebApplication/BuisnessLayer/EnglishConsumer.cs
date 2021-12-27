
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using TRAWebApplication.ConfigSetting;
using TRAWebApplication.Models;

namespace TRAWebApplication.BuisnessLayer
{
    public class EnglishConsumer :  IEnglishConsumer
    {
        OrganizationService organization = new OrganizationService();


        List<string> mappings = new List<string>{
            { "image/bmp" },  // newly added format for zip file 
            { "application/msword"}, // newly added format for zip file 
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }, // newly added format for zip file 
            {"application/pdf"},        // newly added format for pdf file   
            {"image/gif"},
            {"image/jpeg"},
            {"image/jpg" },
            {"image/png"},
            {"application/vnd.ms-powerpoint"},
            {"application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {"application/vnd.ms-xpsdocument"},
            {"image/pjpeg"},
            {"application/octet-stream" },
            {"image/x-png"},
            { "application/x-zip-compressed" },
        };

        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
[MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
[MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I1, SizeParamIndex=3)]
    byte[] pBuffer,
int cbSize,
[MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
int dwMimeFlags,
out IntPtr ppwzMimeOut,
int dwReserved);

        public async Task<List<getTitle>> getTitle()
        {
            List<getTitle> titleList = new List<getTitle>();
            try
            {

                OrganizationService organization = new OrganizationService();

                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {
                    QueryExpression _queryData = new QueryExpression("tra_title");
                    _queryData.ColumnSet = new ColumnSet(true);
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityCase = service.RetrieveMultiple(_queryData);
                    if (_entityCase != null && _entityCase.Entities != null && _entityCase.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityCase.Entities)
                        {

                            getTitle _title = new getTitle();
                            _title.label = item.Contains("tra_name") ? (string)item.Attributes["tra_name"] : string.Empty;
                            _title.value = item.Contains("tra_titleid") ? (Guid)item.Attributes["tra_titleid"] : Guid.Empty;
                            titleList.Add(_title);
                        }

                        if (titleList != null && titleList.Count > 0)
                        {


                            return await Task.Run(() => titleList.OrderBy(a => a.label).Distinct().ToList());

                        }


                    }

                }



            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => titleList.OrderBy(a => a.label).Distinct().ToList());
        }
        public async Task<List<getServiceProviders>> getSrvcProviders()
        {

            List<getServiceProviders> lystservcprdList = new List<getServiceProviders>();
            try
            {

                OrganizationService organization = new OrganizationService();

                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {
                    QueryExpression _queryData = new QueryExpression("tra_serviceprovider");
                    _queryData.ColumnSet = new ColumnSet("tra_name");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityCase = service.RetrieveMultiple(_queryData);
                    if (_entityCase != null && _entityCase.Entities != null && _entityCase.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityCase.Entities)
                        {

                            getServiceProviders _getsrvcPrdInfo = new getServiceProviders();
                            _getsrvcPrdInfo.label = item.Contains("tra_name") ? (string)item.Attributes["tra_name"] : string.Empty;
                            _getsrvcPrdInfo.value = item.Contains("tra_serviceproviderid") ? (Guid)item.Attributes["tra_serviceproviderid"] : Guid.Empty;
                            lystservcprdList.Add(_getsrvcPrdInfo);
                        }

                        if (lystservcprdList != null && lystservcprdList.Count > 0)
                        {


                            return await Task.Run(() => lystservcprdList.OrderBy(a=>a.label).Distinct().ToList());

                        }
                        

                    }
                    
                }



            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => lystservcprdList.OrderBy(a => a.label).Distinct().ToList());
        }

        public async Task<List<servcClass>> getService(string id)
        {
            List<servcClass> servicesList = new List<servcClass>();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {
                    Guid Id = Guid.Parse(id);
                    QueryExpression _queryExp = new QueryExpression("tra_service");
                    _queryExp.ColumnSet = new ColumnSet(new string[]
                                              {
                                              "tra_name"
                                              });
                    FilterExpression filter = new FilterExpression(LogicalOperator.And);
                    filter.AddCondition("tra_serviceprovider", ConditionOperator.Equal, Id);
                    filter.AddCondition("statecode", ConditionOperator.Equal, 0);

                    _queryExp.Criteria = filter;
                    EntityCollection _Entity = service.RetrieveMultiple(_queryExp);
                    if (_Entity != null && _Entity.Entities != null && _Entity.Entities.Count > 0)
                    {
                        foreach (Entity data in _Entity.Entities)
                        {
                            servcClass _srvcData = new servcClass();
                            _srvcData.label = data.Attributes.Contains("tra_name") ? (string)data.Attributes["tra_name"].ToString() : string.Empty;
                            _srvcData.value = data.Attributes.Contains("tra_serviceid") ? (Guid)(data.Attributes["tra_serviceid"]) : Guid.Empty;

                            servicesList.Add(_srvcData);

                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }

            return await Task.Run(() => servicesList.OrderBy(a=>a.label).Distinct().ToList());
        }

        public async Task<List<getComplaint>> complaints()
        {
            List<getComplaint> lystComplaints = new List<getComplaint>();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
               
                if (service != null)
                {
                
                   
                    QueryExpression _queryData = new QueryExpression("tra_complainttype");
                    _queryData.ColumnSet = new ColumnSet("tra_name");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityCase = service.RetrieveMultiple(_queryData);
                    if (_entityCase != null && _entityCase.Entities != null && _entityCase.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityCase.Entities)
                        {
                            getComplaint _getcomlaint = new getComplaint();
                            _getcomlaint.label = item.Contains("tra_name") ? (string)item.Attributes["tra_name"] : string.Empty;
                            _getcomlaint.value = item.Contains("tra_complainttypeid") ? (Guid)item.Attributes["tra_complainttypeid"] : Guid.Empty;
                            lystComplaints.Add(_getcomlaint);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => lystComplaints.OrderBy(a=>a.label).Distinct().ToList());
        }

        public async Task<List<getComplaintSubType>> getComplaintSubTypes(string complaintid)
        {
            List<getComplaintSubType> complaintSubList = new List<getComplaintSubType>();
            try
            {

                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {
                    Guid Id = Guid.Parse(complaintid);
                    QueryExpression _queryExp = new QueryExpression("tra_complaintsubtype");
                    _queryExp.ColumnSet = new ColumnSet(new string[]
                                              {
                                              "tra_name"
                                              });
                    FilterExpression filter = new FilterExpression(LogicalOperator.And);
                    filter.AddCondition("tra_complainttype", ConditionOperator.Equal, Id);
                    filter.AddCondition("statecode", ConditionOperator.Equal, 0);

                    _queryExp.Criteria = filter;
                    EntityCollection _Entity = service.RetrieveMultiple(_queryExp);
                    if (_Entity != null && _Entity.Entities != null && _Entity.Entities.Count > 0)
                    {
                        foreach (Entity data in _Entity.Entities)
                        {
                            getComplaintSubType complaintSub = new getComplaintSubType();
                            complaintSub.label = data.Attributes.Contains("tra_name") ? (string)data.Attributes["tra_name"].ToString() : string.Empty;
                            complaintSub.value = data.Attributes.Contains("tra_complaintsubtypeid") ? (Guid)(data.Attributes["tra_complaintsubtypeid"]) : Guid.Empty;

                            complaintSubList.Add(complaintSub);

                        }
                    }


                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>complaintSubList.OrderBy(a=>a.label).Distinct().ToList());

        }

        public async Task<caseDetail> createEnquiry()
        {
            
            caseDetail _enquiryData = new caseDetail();
           // documentsData _documentsEnq = null;
            List<documentsData> listdocumentsEnq = new List<documentsData>();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {

                    string caseNumber = string.Empty;
                    var httpRequest = HttpContext.Current.Request;

                    if (httpRequest.Files.Count > 0)
                    {
                        //////////////////MIME Type Checking for Uploaded Files/////////////
                        //foreach (string filesItem in httpRequest.Files)
                        //{
                        //    _documentsEnq = new documentsData();
                        //    HttpPostedFile httpPostedFiles = httpRequest.Files[filesItem];
                        //    Stream fs = httpPostedFiles.InputStream;
                        //    BinaryReader br = new System.IO.BinaryReader(fs);
                        //    Byte[] fileBytes = br.ReadBytes((Int32)fs.Length);
                        //    _documentsEnq.documents= Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                        //    listdocumentsEnq.Add(_documentsEnq);
                        //    IntPtr mimeTypePtr;
                        //    FindMimeFromData(IntPtr.Zero, null, fileBytes, 256, null, 0, out mimeTypePtr, 0);
                        //    string mime = Marshal.PtrToStringUni(mimeTypePtr);
                        //    Marshal.FreeCoTaskMem(mimeTypePtr);

                        //    if (!mappings.Contains(mime))
                        //    {
                        //        _enquiryData.errorCode = HttpStatusCode.BadRequest;
                        //        return _enquiryData;
                        //    }


                        //}

                    }

                    if (httpRequest!=null&&httpRequest.Form.Count>0)
                    {
                        string enquiry_typeId = HttpContext.Current.Request.Form["enquiryTypesId"];


                        string subject = httpRequest.Form["subject"];
                        string enquiryDescription = httpRequest.Form["enquiry_desc"];
                        Entity _enquiryInfo = new Entity("incident");

                        if (enquiry_typeId != string.Empty && enquiryDescription != string.Empty)
                        {
                            string Contactid = httpRequest.Form["contactid"];
                            Guid _customerId = Guid.Parse(Contactid);

                            _enquiryInfo["customerid"] = new EntityReference("contact", _customerId);
                            _enquiryInfo["casetypecode"] = new OptionSetValue(2);
                            _enquiryInfo["tra_enquirytype"] = new EntityReference("tra_enquirytype", Guid.Parse(enquiry_typeId));
                            _enquiryInfo["description"] = enquiryDescription.Trim();
                            _enquiryInfo["caseorigincode"] = new OptionSetValue(3);
                           
                            Guid enquiryId = service.Create(_enquiryInfo);

                            if (enquiryId != Guid.Empty)
                            {

                                Entity _case = service.Retrieve("incident", enquiryId, new ColumnSet("ticketnumber"));

                                if (_case != null)
                                {
                                    caseNumber = _case.Attributes["ticketnumber"].ToString();
                                }

                               
                            }
                            /////////////////////create conversation ////////////////////////////////////
                            if (httpRequest.Files.Count > 0)
                            {


                                Entity _conversionEntity = new Entity("tra_conversation");

                                _conversionEntity["tra_caseid"] = new EntityReference("incident", enquiryId);
                                _conversionEntity["tra_comment"] = "Other Supporting Documents (if any)";
                                _conversionEntity.Attributes.Add("tra_commentby", new OptionSetValue(1));
                                _conversionEntity["tra_consumer"] = Convert.ToBoolean(true);
                                _conversionEntity["tra_showonlyattachmentsonportal"] = Convert.ToBoolean(true);
                                Guid conversationId = service.Create(_conversionEntity);

                                if (conversationId != Guid.Empty)
                                {
                                    List<documentBody> lystdocumentBody = new List<documentBody>();
                                    List<postedFile> listPostedFiles = new List<postedFile>();
                                    List<Entity> AnnotationEntityList = new List<Entity>();
                                    postedFile postedFiles = null;
                                    documentBody _documentuploads = null;
                                    foreach (string file in httpRequest.Files)
                                    {
                                        postedFiles = new postedFile();
                                        postedFiles.httppostedFile = httpRequest.Files[file];
                                        postedFiles.filename = postedFiles.httppostedFile.FileName;
                                        listPostedFiles.Add(postedFiles);

                                    }

                                    if (listPostedFiles != null && listPostedFiles.Count > 0)
                                    {

                                        foreach (var items in listPostedFiles)
                                        {
                                            _documentuploads = new documentBody();
                                            Stream fs = items.httppostedFile.InputStream;
                                            BinaryReader br = new System.IO.BinaryReader(fs);
                                            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                            _documentuploads.documentbody = Convert.ToBase64String(bytes, 0, bytes.Length);
                                            _documentuploads.filename = items.filename;
                                            lystdocumentBody.Add(_documentuploads);
                                        }
                                        //if(listdocumentsEnq!=null&& listdocumentsEnq.Count > 0)
                                        //{
                                        //   for(int r = 0; r < listdocumentsEnq.Count; r++)
                                        //    {
                                        //        lystdocumentBody[r].documentbody = listdocumentsEnq[r].documents;
                                        //    }
                                        //}

                                        if (lystdocumentBody != null && lystdocumentBody.Count > 0)
                                        {
                                            for (int i = 0; i < lystdocumentBody.Count; i++)
                                            {
                                                Entity annotationEntity = new Entity("annotation");
                                                annotationEntity["documentbody"] = lystdocumentBody[i].documentbody;
                                                annotationEntity["filename"] = lystdocumentBody[i].filename;
                                                AnnotationEntityList.Add(annotationEntity);

                                            }

                                            if (AnnotationEntityList != null && AnnotationEntityList.Count > 0)
                                            {
                                                int m = 0;
                                                foreach (var dataAttachment in AnnotationEntityList)
                                                {
                                                    AnnotationEntityList[m].Attributes.Add("objectid", new EntityReference("tra_conversation", conversationId));
                                                    service.Create(AnnotationEntityList[m]);
                                                    m++;
                                                }

                                                _enquiryData.caseNo = caseNumber;
                                                _enquiryData.caseId = enquiryId;
                                                _enquiryData.errorCode = HttpStatusCode.OK;
                                                _enquiryData.message = "Successfully created Enquiry with files";
                                            }
                                        }

                                    }
                                }
                            }
                            else
                            {
                                _enquiryData.caseNo = caseNumber;
                                _enquiryData.caseId = enquiryId;
                                _enquiryData.errorCode = HttpStatusCode.OK;
                                _enquiryData.message = "Successfully created Enquiry without files";
                            }

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }

            return await Task.Run(()=>_enquiryData);

        }

        public async Task<caseDetail> createSuggestion()
        {
            caseDetail suggestionInfos = new caseDetail();
            documentsData documentsSugg = null;
            List<documentsData> listDocuSugg = new List<documentsData>();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {
                    string enquiryTypeId = string.Empty;
                    string caseNumber = string.Empty;
                    var httpRequest = HttpContext.Current.Request;

                    if (httpRequest.Files.Count > 0)
                    {
                        //////////////////MIME Type Checking for Uploaded Files/////////////
                        //foreach (string filesItem in httpRequest.Files)
                        //{
                        //    documentsSugg = new documentsData();
                        //    HttpPostedFile httpPostedFiles = httpRequest.Files[filesItem];
                        //    Stream fs = httpPostedFiles.InputStream;
                        //    BinaryReader br = new System.IO.BinaryReader(fs);
                        //    Byte[] fileBytes = br.ReadBytes((Int32)fs.Length);
                        //    documentsSugg.documents= Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                        //    listDocuSugg.Add(documentsSugg);
                        //    IntPtr mimeTypePtr;
                        //    FindMimeFromData(IntPtr.Zero, null, fileBytes, 256, null, 0, out mimeTypePtr, 0);
                        //    string mime = Marshal.PtrToStringUni(mimeTypePtr);
                        //    Marshal.FreeCoTaskMem(mimeTypePtr);

                        //    if (!mappings.Contains(mime))
                        //    {
                        //        suggestionInfos.errorCode = HttpStatusCode.BadRequest;
                        //        return suggestionInfos;
                        //    }


                        //}

                    }

                        if (httpRequest!=null&&httpRequest.Form.Count>0)
                        {

                            string subject = httpRequest.Form["subject"];
                            string suggestions = httpRequest.Form["suggestions"];
                            Entity suggesionsData = new Entity("incident");

                           if (suggestions != string.Empty)
                           {
                            string Contactid = httpRequest.Form["contactid"];
                            Guid _customerId = Guid.Parse(Contactid);

                            suggesionsData["customerid"] = new EntityReference("contact", _customerId);
                            suggesionsData["casetypecode"] = new OptionSetValue(3);

                            suggesionsData["description"] = suggestions.Trim();
                            suggesionsData["caseorigincode"] = new OptionSetValue(3);


                             Guid suggestionId = service.Create(suggesionsData);

                               if (suggestionId != Guid.Empty)
                               {
                                   Entity _SuggestionEntity = service.Retrieve("incident", suggestionId, new ColumnSet("ticketnumber"));
                                   if (_SuggestionEntity != null)
                                   {
                                       caseNumber = _SuggestionEntity.Attributes["ticketnumber"].ToString();
                                   }

                               }
                            /////////////////////create conversation ////////////////////////////////////

                            if (httpRequest.Files.Count > 0)
                            {

                                Entity _conversionEntity = new Entity("tra_conversation");

                                _conversionEntity["tra_caseid"] = new EntityReference("incident", suggestionId);
                                _conversionEntity["tra_comment"] = "Other Supporting Documents (if any)";
                                _conversionEntity["tra_showonlyattachmentsonportal"] = Convert.ToBoolean(true);
                                _conversionEntity.Attributes.Add("tra_commentby", new OptionSetValue(1));
                                _conversionEntity["tra_consumer"] = Convert.ToBoolean(true);
                                Guid conversationId = service.Create(_conversionEntity);

                                if (conversationId != Guid.Empty)
                                {
                                    List<documentBody> lystdocumentBody = new List<documentBody>();
                                    List<postedFile> listPostedFiles = new List<postedFile>();
                                    List<Entity> AnnotationEntityList = new List<Entity>();
                                    postedFile postedFiles = null;
                                    documentBody _documentuploads = null;
                                    foreach (string file in httpRequest.Files)
                                    {
                                        postedFiles = new postedFile();
                                        postedFiles.httppostedFile = httpRequest.Files[file];
                                        postedFiles.filename = postedFiles.httppostedFile.FileName;
                                        listPostedFiles.Add(postedFiles);

                                    }

                                    if (listPostedFiles != null && listPostedFiles.Count > 0)
                                    {

                                        foreach (var items in listPostedFiles)
                                        {
                                            _documentuploads = new documentBody();
                                            Stream fs = items.httppostedFile.InputStream;
                                            BinaryReader br = new System.IO.BinaryReader(fs);
                                            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                            _documentuploads.documentbody = Convert.ToBase64String(bytes, 0, bytes.Length);
                                            _documentuploads.filename = items.filename;
                                            lystdocumentBody.Add(_documentuploads);
                                        }

                                        //if (listDocuSugg != null && listDocuSugg.Count > 0)
                                        //{
                                        //    for(int e = 0; e < listDocuSugg.Count; e++)
                                        //    {
                                        //        lystdocumentBody[e].documentbody = listDocuSugg[e].documents;
                                        //    }
                                        //}

                                        if (lystdocumentBody != null && lystdocumentBody.Count > 0)
                                        {
                                            for (int i = 0; i < lystdocumentBody.Count; i++)
                                            {
                                                Entity annotationEntity = new Entity("annotation");
                                                annotationEntity["documentbody"] = lystdocumentBody[i].documentbody;
                                                annotationEntity["filename"] = lystdocumentBody[i].filename;
                                                AnnotationEntityList.Add(annotationEntity);

                                            }

                                            if (AnnotationEntityList != null && AnnotationEntityList.Count > 0)
                                            {
                                                int m = 0;
                                                foreach (var dataAttachment in AnnotationEntityList)
                                                {
                                                    AnnotationEntityList[m].Attributes.Add("objectid", new EntityReference("tra_conversation", conversationId));
                                                    service.Create(AnnotationEntityList[m]);
                                                    m++;
                                                }

                                                suggestionInfos.caseNo = caseNumber;

                                                suggestionInfos.errorCode = HttpStatusCode.OK;
                                                suggestionInfos.message = "Successfully created Suggestions with files";
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                suggestionInfos.caseNo = caseNumber;
                                suggestionInfos.errorCode = HttpStatusCode.OK;
                                suggestionInfos.message = "Successfully created Suggestions without files";
                            }

                        }

                    }

                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => suggestionInfos);
        }

        public async Task<caseDetail> Addcomplaints()
        {

            caseDetail complaintInfo = new caseDetail();
            ComplaintObject _complaintObj = new ComplaintObject();
           // documentsData _documentsData = null;
            List<documentsData> _listDocuments = new List<documentsData>();

            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);


                if (service != null)
                {
                    var httpRequestData = HttpContext.Current.Request;


                    if (httpRequestData!=null&&httpRequestData.Form.Count>0)
                    {
                        //////////////////MIME Type Checking for Uploaded Files/////////////
                        //foreach (string filesItem in httpRequestData.Files)
                        //{
                        //    _documentsData = new documentsData();
                        //    HttpPostedFile httpPostedFiles = httpRequestData.Files[filesItem];
                        //    Stream fs = httpPostedFiles.InputStream;
                        //    BinaryReader br = new System.IO.BinaryReader(fs);
                        //    Byte[] fileBytes = br.ReadBytes((Int32)fs.Length);
                        //    _documentsData.documents= Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                        //    _listDocuments.Add(_documentsData);
                        //    IntPtr mimeTypePtr;
                        //    FindMimeFromData(IntPtr.Zero, null, fileBytes, 256, null, 0, out mimeTypePtr, 0);
                        //    string mime = Marshal.PtrToStringUni(mimeTypePtr);
                        //    Marshal.FreeCoTaskMem(mimeTypePtr);

                        //    if (!mappings.Contains(mime))
                        //    {
                        //        complaintInfo.errorCode = HttpStatusCode.BadRequest;
                        //        return complaintInfo;
                        //    }


                        //}

                        _complaintObj.contactId = HttpContext.Current.Request.Form["contactid"];
                        _complaintObj.complaintType = HttpContext.Current.Request.Form["complaintType"];
                        _complaintObj.complaintSubType = HttpContext.Current.Request.Form["complaintSubType"];
                        _complaintObj.srcvProvider = HttpContext.Current.Request.Form["srcvProvider"];
                        _complaintObj.services = HttpContext.Current.Request.Form["services"];
                        _complaintObj.flat = HttpContext.Current.Request.Form["flat"];
                        _complaintObj.buildingNo = HttpContext.Current.Request.Form["buildingNo"];
                        _complaintObj.roadStreet = HttpContext.Current.Request.Form["roadStreet"];
                        _complaintObj.blockNo = HttpContext.Current.Request.Form["blockNo"];
                        _complaintObj.complaintDescription = HttpContext.Current.Request.Form["complaintDescription"];
                        _complaintObj.spResponse = HttpContext.Current.Request.Form["spResponse"];
                        _complaintObj.solutionSatisfy = HttpContext.Current.Request.Form["solutionSatisfy"];
                        _complaintObj.disputeNo = HttpContext.Current.Request.Form["disputeNo"];

                        _complaintObj.subscriptionType = HttpContext.Current.Request.Form["subscriptionType"];
                       _complaintObj.complaintDate = HttpContext.Current.Request.Form["complaintDate"];
                        _complaintObj.caseRefNo = HttpContext.Current.Request.Form["caseRefNo"];
                        _complaintObj.ownerNm = HttpContext.Current.Request.Form["ownerNm"];
                       
                        _complaintObj.cprNoofowner = HttpContext.Current.Request.Form["cprNoofowner"];
                        _complaintObj.tra_ownerofdisputednumber = HttpContext.Current.Request.Form["tra_ownerofdisputednumber"];
                        _complaintObj.complaintName = HttpContext.Current.Request.Form["complaintypename"];
                        _complaintObj.disputedAmount = HttpContext.Current.Request.Form["tra_disputedamount"];
                        _complaintObj.voice = HttpContext.Current.Request.Form["voice"];
                        _complaintObj.data = HttpContext.Current.Request.Form["data"];


                        if (_complaintObj != null)
                        {
                            Entity _complaintEntity = new Entity();
                            _complaintEntity.LogicalName = "incident";


                            Guid _customerId = Guid.Parse(_complaintObj.contactId);

                            _complaintEntity["customerid"] = new EntityReference("contact", _customerId);
                            _complaintEntity["casetypecode"] = new OptionSetValue(Convert.ToInt32(1));
                            _complaintEntity["tra_serviceprovider"] = new EntityReference("tra_serviceprovider", Guid.Parse(_complaintObj.srcvProvider));
                            _complaintEntity["tra_service"] = new EntityReference("tra_service", Guid.Parse(_complaintObj.services));
                            _complaintEntity["tra_complainttype"] = new EntityReference("tra_complainttype", Guid.Parse(_complaintObj.complaintType));

                            if(_complaintObj.complaintSubType!= "undefined")
                            {
                                _complaintEntity["tra_complaintsubtype"] = new EntityReference("tra_complaintsubtype", Guid.Parse(_complaintObj.complaintSubType));
                            }
                          
                            _complaintEntity["tra_serviceprovidercasereference"] = _complaintObj.caseRefNo.Trim();
                            
                            _complaintEntity["tra_disputednumber"] = _complaintObj.disputeNo.Trim();
                           
                            _complaintEntity["caseorigincode"] = new OptionSetValue(3);

                            if (_complaintObj.subscriptionType == "Postpaid")
                            {
                                _complaintEntity["tra_subscriptiontype"] = new OptionSetValue(Convert.ToInt32(167490001));
                            }
                            else if (_complaintObj.subscriptionType == "Prepaid")
                            {
                                _complaintEntity["tra_subscriptiontype"] = new OptionSetValue(Convert.ToInt32(167490000));
                            }
                            _complaintEntity["tra_dateofcomplaintagainstserviceprovider"] = Convert.ToDateTime(_complaintObj.complaintDate);
                            _complaintEntity["tra_casequestion1"] = _complaintObj.complaintDescription.Trim();
                            _complaintEntity["tra_casequestion2"] = _complaintObj.spResponse.Trim();
                            _complaintEntity["tra_casequestion3"] = _complaintObj.solutionSatisfy.Trim();

                            if (_complaintObj.tra_ownerofdisputednumber == "false")
                            {
                                _complaintEntity["tra_ownerofdisputednumber"] = Convert.ToBoolean(false);
                                _complaintEntity["tra_nameofowner"] = _complaintObj.ownerNm.Trim();
                                _complaintEntity["tra_cprnumberofowner"] = _complaintObj.cprNoofowner.Trim();

                            }
                            else if (_complaintObj.tra_ownerofdisputednumber == "true")
                            {

                                _complaintEntity["tra_ownerofdisputednumber"] = Convert.ToBoolean(true);
                                _complaintEntity["tra_nameofowner"] = string.Empty;
                                _complaintEntity["tra_cprnumberofowner"] = string.Empty;

                            }

                            if (_complaintObj.complaintName == "Billing")
                            {
                                string disputeAmount = _complaintObj.disputedAmount.Replace("BHD", "");
                                decimal amount = Convert.ToDecimal(disputeAmount);

                                _complaintEntity["tra_disputedamount"] =new Money(amount) ;
                            }
                            
                            else if (_complaintObj.complaintName == "Quality of Service")
                            {
                                _complaintEntity["tra_flat"] = _complaintObj.flat.Trim();
                                _complaintEntity["tra_buildingflat"] = _complaintObj.buildingNo.Trim();
                                _complaintEntity["tra_street"] = _complaintObj.roadStreet.Trim();
                                _complaintEntity["tra_block"] = _complaintObj.blockNo.Trim();

                                if (_complaintObj.voice == "voice")
                                {
                                    _complaintEntity["tra_voice"] = Convert.ToBoolean(true);
                                }
                                else if (_complaintObj.voice == "undefined")
                                {
                                    _complaintEntity["tra_voice"] = Convert.ToBoolean(false);
                                }

                                if (_complaintObj.data == "data")
                                {
                                    _complaintEntity["tra_data"] = Convert.ToBoolean(true);
                                }
                                else if (_complaintObj.data == "undefined")
                                {
                                    _complaintEntity["tra_data"] = Convert.ToBoolean(false);
                                }
                            }

                            Guid complaintId = service.Create(_complaintEntity);
                            
                            string caseNum = string.Empty;

                            if (complaintId != Guid.Empty)
                            {

                                Entity _complaint = service.Retrieve("incident", complaintId, new ColumnSet("ticketnumber"));
                                if (_complaint != null)
                                {

                                    caseNum = _complaint.Attributes["ticketnumber"].ToString();
                                }
                      
                                if (caseNum != string.Empty&& httpRequestData.Files.Count>0)
                                {

                                    List<documentBody> lystdocumentBody = new List<documentBody>();

                                    List<postedFile> listPostedFiles = new List<postedFile>();
                                    List<postedFile> listPostedFiles1 = new List<postedFile>();
                                    List<Entity> AnnotationEntityList = new List<Entity>();
                                    List<Entity> AnnotationEntityList1 = new List<Entity>();
                                    List<Entity> ConversationEntityList = new List<Entity>();
                                    postedFile postedFiles = null;
                                    postedFile postedFiles1 = null;
                                    documentBody _documentuploads = null;


                                    foreach (string file in httpRequestData.Files)
                                    {
                                        postedFiles = new postedFile();
                                        postedFiles.httppostedFile = httpRequestData.Files[file];
                                        postedFiles.filename = postedFiles.httppostedFile.FileName;
                                        listPostedFiles.Add(postedFiles);

                                    }

                                    for (int iy = 0; iy < httpRequestData.Files.Count; iy++)
                                    {
                                        postedFiles1 = new postedFile();
                                        postedFiles1.documntsName = HttpContext.Current.Request.Form["documents" + iy + ""];
                                        listPostedFiles1.Add(postedFiles1);
                                    }
                                    for (int g = 0; g < listPostedFiles1.Count; g++)
                                    {
                                        listPostedFiles[g].documntsName = listPostedFiles1[g].documntsName;
                                    }

                                    if (listPostedFiles != null && listPostedFiles.Count > 0)
                                    {

                                        foreach (var items in listPostedFiles)
                                        {
                                            _documentuploads = new documentBody();
                                            Stream fs = items.httppostedFile.InputStream;
                                            BinaryReader br = new System.IO.BinaryReader(fs);
                                            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                            _documentuploads.documentbody = Convert.ToBase64String(bytes, 0, bytes.Length);
                                            _documentuploads.filename = items.filename;
                                            _documentuploads.documentType = items.documntsName;
                                            lystdocumentBody.Add(_documentuploads);
                                        }
                                        //if(_listDocuments!=null&& _listDocuments.Count > 0)
                                        //{
                                        //    for (int y = 0; y < _listDocuments.Count; y++)
                                        //    {
                                        //        lystdocumentBody[y].documentbody = _listDocuments[y].documents;
                                        //    }
                                        //}
                                       
                                        
                                        if (lystdocumentBody != null && lystdocumentBody.Count > 0)
                                        {
                                            for (int i = 0; i < lystdocumentBody.Count; i++)
                                            {
                                                if (lystdocumentBody[i].documentType != "Other Supporting Documents (if any)")
                                                {

                                                    Entity conversation = new Entity("tra_conversation");
                                                    conversation.Attributes.Add("tra_comment", lystdocumentBody[i].documentType);
                                                    conversation.Attributes.Add("tra_showonlyattachmentsonportal", true);
                                                    conversation.Attributes.Add("tra_consumer", true);
                                                    conversation.Attributes.Add("tra_commentby", new OptionSetValue(1));

                                                    ConversationEntityList.Add(conversation);


                                                    //////////////////////Annotation Entity/////////////////////////////
                                                    Entity ann = new Entity("annotation");

                                                    ann.Attributes.Add("subject", "consumer - attachment uploaded");

                                                    ann.Attributes.Add("filename", lystdocumentBody[i].filename);
                                                    ann.Attributes.Add("documentbody", lystdocumentBody[i].documentbody);

                                                    AnnotationEntityList.Add(ann);
                                                }
                                            }

                                            var filesData = lystdocumentBody.Where(a => a.documentType == "Other Supporting Documents (if any)").ToList();
                                            if (filesData != null && filesData.Count > 0)
                                            {
                                                Entity _conversionEntity = new Entity("tra_conversation");

                                                _conversionEntity["tra_caseid"] = new EntityReference("incident", complaintId);
                                                _conversionEntity["tra_comment"] = "Other Supporting Documents (if any)";
                                                _conversionEntity["tra_consumer"] = Convert.ToBoolean(true);
                                                _conversionEntity.Attributes.Add("tra_commentby", new OptionSetValue(1));
                                                _conversionEntity["tra_showonlyattachmentsonportal"] = Convert.ToBoolean(true);
                                                Guid conversationIdforMultiples = service.Create(_conversionEntity);

                                                if (conversationIdforMultiples != Guid.Empty)
                                                {
                                                    for (int ij = 0; ij < filesData.Count; ij++)
                                                    {
                                                        if (filesData[ij].documentType == "Other Supporting Documents (if any)" && filesData[ij].documentType.Contains("Other Supporting Documents (if any)"))
                                                        {
                                                            Entity annotationEntity1 = new Entity("annotation");
                                                            annotationEntity1["documentbody"] = filesData[ij].documentbody;
                                                            annotationEntity1["filename"] = filesData[ij].filename;
                                                            annotationEntity1.Attributes.Add("subject", "consumer - attachment uploaded");
                                                            AnnotationEntityList1.Add(annotationEntity1);
                                                        }

                                                    }

                                                    if (AnnotationEntityList1 != null && AnnotationEntityList1.Count > 0)
                                                    {
                                                        int ms = 0;
                                                        foreach (var dataAttachment in AnnotationEntityList1)
                                                        {
                                                            AnnotationEntityList1[ms].Attributes.Add("objectid", new EntityReference("tra_conversation", conversationIdforMultiples));
                                                            service.Create(AnnotationEntityList1[ms]);
                                                            ms++;
                                                        }
                                                    }
                                                }

                                            }

                                            if (AnnotationEntityList != null && AnnotationEntityList.Count > 0 && ConversationEntityList != null && ConversationEntityList.Count > 0)
                                            {

                                                int cnt = 0;
                                                foreach (var conversationItem in ConversationEntityList)
                                                {

                                                    conversationItem.Attributes.Add("tra_caseid", new EntityReference("incident", complaintId));
                                                    Guid converId = service.Create(conversationItem);

                                                    AnnotationEntityList[cnt].Attributes.Add("objectid", new EntityReference("tra_conversation", converId));
                                                    service.Create(AnnotationEntityList[cnt]);
                                                    cnt++;

                                                }
                                                complaintInfo.caseNo = caseNum;
                                                complaintInfo.errorCode = HttpStatusCode.OK;
                                                complaintInfo.caseId = complaintId;
                                                complaintInfo.message = "Complaint created successfully";
                                            }
                                        }
                                    }


                                }
                                else
                                {
                                    complaintInfo.caseNo = caseNum;
                                    complaintInfo.errorCode = HttpStatusCode.OK;
                                    complaintInfo.caseId = complaintId;
                                    complaintInfo.message = "Complaint created successfully";
                                }

                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => complaintInfo);
        }

       public async Task<List<myCases>> mycases(string contactid)
       {
            myCases cases1 = null;
            myCases cases = null;
            List<myCases> data = new List<myCases>();
            List<myCases> caseList = new List<myCases>();

            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {
                    if (!string.IsNullOrEmpty(contactid))
                    {
                        Guid _customerId = Guid.Parse(contactid);

                        QueryExpression _incidentData = new QueryExpression("incident");
                        _incidentData.ColumnSet = new ColumnSet(new string[]
                        {
                                "ticketnumber","createdon","casetypecode","customerid","statuscode","tra_casestatusforconsumer"

                        });

                        FilterExpression _filterIncident = new FilterExpression(LogicalOperator.And);
                        _filterIncident.AddCondition("customerid", ConditionOperator.Equal, _customerId);
                        _incidentData.Criteria = _filterIncident;

                        EntityCollection _consumerCase = service.RetrieveMultiple(_incidentData);

                        if (_consumerCase != null && _consumerCase.Entities != null && _consumerCase.Entities.Count > 0)
                        {

                            foreach (Entity item in _consumerCase.Entities)
                            {
                                cases = new myCases();
                                if (_customerId == ((EntityReference)item["customerid"]).Id)
                                {
                                    cases.case_refNo = item.Contains("ticketnumber") ? (string)item.Attributes["ticketnumber"] : string.Empty;
                                    cases.caseId = (Guid)item.Attributes["incidentid"];
                                    cases.createdDate = (DateTime)item.Attributes["createdon"];
                                    DateTime dt = (DateTime)item.Attributes["createdon"];
                                    cases.date_submitted = dt.ToString("dd/MM/yyy", CultureInfo.InvariantCulture);
                                    cases.case_status = item.Attributes.Contains("tra_casestatusforconsumer") ? (string)item.FormattedValues["tra_casestatusforconsumer"].ToString() : string.Empty;
                                    cases.case_type = item.Attributes.Contains("casetypecode") ? (string)item.FormattedValues["casetypecode"] : string.Empty;

                                    data.Add(cases);

                                }


                            }

                            caseList = data.OrderByDescending(a => a.createdDate).Distinct().ToList();
                          
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => caseList);
           
        }
        public async Task<caseInformation> caseDetails(string caseno)
        {
            caseInformation caseInfos = new caseInformation();
            try
            {
                if (!string.IsNullOrEmpty(caseno))
                {
                    caseDetails _caseinfo = null;
                    annotationClass annotation = null;
                    conversationView _conversationView = null;
                    conversationView _conversationView1 = null;


                    List<annotationClass> annotationColl = new List<annotationClass>();
                    List<conversationView> _listConversation = new List<conversationView>();
                    List<conversationView> _listConversation1 = new List<conversationView>();

                    OrganizationService organization = new OrganizationService();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    IOrganizationService service = organization.GetCRMService(configData);

                    if (service != null)
                    {


                        QueryExpression _queryData = new QueryExpression("incident");
                        _queryData.ColumnSet = new ColumnSet(new string[]
                        {
                         "ticketnumber","createdon","casetypecode","statecode","tra_casequestion1","tra_casestatusforconsumer","description"

                        });
                        FilterExpression filterData = new FilterExpression(LogicalOperator.And);
                        filterData.AddCondition("ticketnumber", ConditionOperator.Equal, caseno);

                        _queryData.Criteria = filterData;
                        EntityCollection _entity = service.RetrieveMultiple(_queryData);
                        if (_entity != null && _entity.Entities != null && _entity.Entities.Count > 0)
                        {

                            foreach (Entity item in _entity.Entities)
                            {
                                _caseinfo = new caseDetails();
                                _caseinfo.caseId = item.Contains("incidentid") ? (Guid)item.Attributes["incidentid"] : Guid.Empty;
                                DateTime date = (DateTime)item.Attributes["createdon"];
                                _caseinfo.createOn = date.ToString("dd/MM/yyy", CultureInfo.InvariantCulture);
                                _caseinfo.caseNo = item.Contains("ticketnumber") ? (string)item.Attributes["ticketnumber"] : string.Empty;
                                _caseinfo.caseType = item.Contains("casetypecode") ? (string)item.FormattedValues["casetypecode"] : string.Empty;
                                _caseinfo.status = item.Contains("tra_casestatusforconsumer") ? (string)item.FormattedValues["tra_casestatusforconsumer"] : string.Empty;
                                _caseinfo.stateCode = item.Contains("statecode") ? (string)item.FormattedValues["statecode"] : string.Empty;

                                if (_caseinfo.caseType == "Complaint")
                                {
                                    _caseinfo.caseDescription = item.Contains("tra_casequestion1") ? (string)item.Attributes["tra_casequestion1"] : string.Empty;
                                }
                                else if(_caseinfo.caseType == "Enquiry"|| _caseinfo.caseType == "Suggestion")
                                {
                                    _caseinfo.caseDescription = item.Contains("description") ? (string)item.Attributes["description"] : string.Empty;
                                }
                            }

                            caseInfos._caseData = _caseinfo;

                            if (_caseinfo != null)
                            {

                                QueryExpression convQuery = new QueryExpression("tra_conversation");
                                convQuery.ColumnSet = new ColumnSet(true);

                                FilterExpression filterConversation = new FilterExpression(LogicalOperator.And);
                                filterConversation.AddCondition("tra_consumer", ConditionOperator.Equal, true);                         
                                filterConversation.AddCondition("tra_caseid", ConditionOperator.Equal, _caseinfo.caseId);
                                convQuery.Criteria = filterConversation;


                                EntityCollection _conversationCollection = service.RetrieveMultiple(convQuery);
                                
                                if (_conversationCollection.Entities != null && _conversationCollection.Entities.Count > 0)
                                {
                                    foreach (Entity itemData in _conversationCollection.Entities)
                                    {
                                          _conversationView = new conversationView();
                                        

                                            _conversationView.comments = itemData.Contains("tra_comment") ? (string)itemData.Attributes["tra_comment"] : string.Empty;
                                            _conversationView.commentBy = itemData.Contains("tra_commentby") ? (string)itemData.FormattedValues["tra_commentby"] : string.Empty;
                                            _conversationView.createdOn = (DateTime)itemData.Attributes["createdon"];
                                            _conversationView.conversationId = (Guid)itemData.Attributes["tra_conversationid"];
                                        _conversationView.IsConsumer = itemData.Contains("tra_consumer") ? (Boolean)itemData.Attributes["tra_consumer"] : false;
                                        _conversationView.isPortal= itemData.Contains("tra_showonlyattachmentsonportal") ?(Boolean)itemData.Attributes["tra_showonlyattachmentsonportal"]:false;

                                        _listConversation.Add(_conversationView);
                                       
                                       
                                    }


                                    if (_listConversation != null && _listConversation.Count > 0)
                                    {

                                        foreach (var conversationItems in _listConversation)
                                        {

                                            if ((conversationItems.commentBy == "Consumer"&&conversationItems.isPortal==true)||(conversationItems.IsConsumer == true))
                                            {
                                                QueryExpression annotQuery = new QueryExpression("annotation");
                                                annotQuery.ColumnSet = new ColumnSet(new string[]
                                                {
                                                    "filename"
                                                });
                                                FilterExpression _annotFilters = new FilterExpression(LogicalOperator.And);
                                                _annotFilters.AddCondition("objectid", ConditionOperator.Equal, conversationItems.conversationId);
                                                annotQuery.Criteria = _annotFilters;
                                                EntityCollection _annotationCollections = service.RetrieveMultiple(annotQuery);

                                                if (_annotationCollections.Entities != null && _annotationCollections.Entities.Count > 0)
                                                {
                                                    foreach (Entity annotItems in _annotationCollections.Entities)
                                                    {
                                                        annotation = new annotationClass();
                                                        annotation.annotId = (Guid)annotItems.Attributes["annotationid"];                         
                                                        annotation.filename = annotItems.Contains("filename")? annotItems.Attributes["filename"].ToString():string.Empty;
                                                        annotation.commentBy = conversationItems.commentBy;
                                                        annotation.IsportalAttach = conversationItems.isPortal;
                                                        annotation.isConsumer = conversationItems.IsConsumer;
                                                        annotationColl.Add(annotation);
                                                    }

                                                    caseInfos.listAnnotation = annotationColl;
                                                }
                                            }

                                        }

                                        foreach (var item in _listConversation)
                                        {
                                            _conversationView1 = new conversationView();

                                            if (item.isPortal == false)
                                            {
                                                _conversationView1.commentBy = item.commentBy;
                                                _conversationView1.comments = item.comments;
                                                _conversationView1.conversationId = item.conversationId;
                                                _conversationView1.createdOn = item.createdOn;
                                                _conversationView1.isPortal = item.isPortal;
                                                _listConversation1.Add(_conversationView1);
                                            }
                                        }

                                        var commentListFromConsumer = _listConversation1.OrderByDescending(a => a.createdOn).Distinct().ToList();
                                       
                                        caseInfos.CommentbyListFromConsumer = commentListFromConsumer;
                                      
                                  
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>caseInfos);
        }
        public async Task<List<documentType>> documentType(string complaintid)
        {
            List<documentType> listDocuments = new List<documentType>();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {

                    documentType document = null;
                    string fetchDocuments = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                              <entity name='tra_documenttype'>
                                              <attribute name='tra_documenttypeid' />
                                             <attribute name='tra_name' />
                                             <attribute name='createdon' />
                                             <order attribute='tra_name' descending='false' />
                                         <link-entity name='tra_documentcontrol_tra_documenttype' from='tra_documenttypeid' to='tra_documenttypeid' visible='false' intersect='true'>
                                        <link-entity name='tra_documentcontrol' from='tra_documentcontrolid' to='tra_documentcontrolid' alias='ab'>
                                     <filter type='and'>
                                  <condition attribute='tra_complainttype' operator='eq'  uitype='tra_complainttype' value='{" + complaintid + @"}' />
                                  </filter>
                                 </link-entity>
                                </link-entity>
                                   </entity>
                                  </fetch>";

                    EntityCollection documetsCollection = service.RetrieveMultiple(new FetchExpression(fetchDocuments));

                    if (documetsCollection != null && documetsCollection.Entities != null && documetsCollection.Entities.Count > 0)
                    {

                        foreach (Entity item in documetsCollection.Entities)
                        {
                            document = new documentType();
                            document.document = item.Contains("tra_name") ? (string)item.Attributes["tra_name"] : string.Empty;
                            document.documentId = item.Contains("tra_documenttypeid") ? (Guid)item.Attributes["tra_documenttypeid"] : Guid.Empty;
                            listDocuments.Add(document);
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }

            return await Task.Run(()=>listDocuments);
        }
        public async Task<List<enquiryType>> getEnquiry()
        {
            List<enquiryType> listEnquiry = new List<enquiryType>();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);


                if (service != null)
                {

                    QueryExpression _queryEnquiry = new QueryExpression("tra_enquirytype");
                    _queryEnquiry.ColumnSet = new ColumnSet("tra_name");
                    FilterExpression _filterEnquiry = new FilterExpression(LogicalOperator.And);
                    _filterEnquiry.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryEnquiry.Criteria = _filterEnquiry;
                    EntityCollection _entityEnquiry = service.RetrieveMultiple(_queryEnquiry);
                    if (_entityEnquiry != null && _entityEnquiry.Entities != null && _entityEnquiry.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityEnquiry.Entities)
                        {

                            enquiryType _enquiry = new enquiryType();
                            _enquiry.label = item.Contains("tra_name") ? (string)item.Attributes["tra_name"] : string.Empty;
                            _enquiry.value = item.Contains("tra_enquirytypeid") ? (Guid)item.Attributes["tra_enquirytypeid"] : Guid.Empty;
                            listEnquiry.Add(_enquiry);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;

            }
            return await Task.Run(()=>listEnquiry.OrderBy(a=>a.label).Distinct().ToList());
        }

        public async Task<annotationClass> downloadAttachments(string annotationid)
        {
            annotationClass annotation = null;
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {
                    QueryExpression annotQuery = new QueryExpression("annotation");
                    annotQuery.ColumnSet = new ColumnSet(new string[]
                    {
                   "documentbody","filename"
                    });
                    FilterExpression _annotFilters = new FilterExpression(LogicalOperator.And);
                    _annotFilters.AddCondition("annotationid", ConditionOperator.Equal, Guid.Parse(annotationid));
                    annotQuery.Criteria = _annotFilters;
                    EntityCollection _annotationCollections = service.RetrieveMultiple(annotQuery);

                    if (_annotationCollections.Entities != null && _annotationCollections.Entities.Count > 0)
                    {
                        
                            annotation = new annotationClass();
                            annotation.annotId = (Guid)_annotationCollections.Entities[0].Attributes["annotationid"];
                            annotation.documentbody =_annotationCollections.Entities[0].Contains("documentbody") ? _annotationCollections.Entities[0].Attributes["documentbody"].ToString():string.Empty;
                            annotation.filename = _annotationCollections.Entities[0].Contains("filename") ? _annotationCollections.Entities[0].Attributes["filename"].ToString():string.Empty;
                            annotation.mimeType = GetMimeTypeByWindowsRegistry(annotation.filename);

                    }
                }

            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => annotation);
        }
        public async Task<List<GetNationality>> getNations()
        {
            List<GetNationality> lystNations = new List<GetNationality>();
            try
            {
               
                GetNationality _getNation = null;

                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {

                    QueryExpression _queryData = new QueryExpression("tra_nationality");
                    _queryData.ColumnSet = new ColumnSet("tra_name");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityNation = service.RetrieveMultiple(_queryData);
                    if (_entityNation != null && _entityNation.Entities != null && _entityNation.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityNation.Entities)
                        {
                            _getNation = new GetNationality();
                            _getNation.label = item.Contains("tra_name") ? (string)item.Attributes["tra_name"] : string.Empty;
                            _getNation.value = item.Contains("tra_nationalityid") ? (Guid)item.Attributes["tra_nationalityid"] : Guid.Empty;
                            lystNations.Add(_getNation);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
           
            return await Task.Run(()=> lystNations.OrderBy(a => a.label).Distinct().ToList());
        }

        public async Task<consumerSuccess> indivisualUser(TraUser user)
        {
            consumerSuccess consumers = new consumerSuccess();
            try
            {

                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {

                    if (user != null)
                    {

                        Entity _contactEntity = new Entity("contact");

                        _contactEntity["firstname"] = user.firstname.Trim();
                        _contactEntity["lastname"] = user.lastname.Trim();
                        _contactEntity["tra_source"] = new OptionSetValue(1);

                        if (user.emailId.Trim() != "")
                        {
                            QueryExpression _emailExist = new QueryExpression("contact");
                            _emailExist.ColumnSet = new ColumnSet("emailaddress1");
                            FilterExpression _emailFilters = new FilterExpression(LogicalOperator.And);
                            _emailFilters.AddCondition("emailaddress1", ConditionOperator.Equal, user.emailId.Trim());
                            _emailExist.Criteria = _emailFilters;
                            EntityCollection _emailsColl = service.RetrieveMultiple(_emailExist);
                            if (_emailsColl.Entities != null && _emailsColl.Entities.Count > 0)
                            {
                                consumers.errorCode = HttpStatusCode.NotAcceptable;
                                consumers.message = "Email Exist";
                                return consumers;

                            }
                            else
                            {
                                _contactEntity["emailaddress1"] = user.emailId.Trim();
                            }

                        }
                     

                        if (user.IdType == "CPR Number")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(1);

                        }
                        else if (user.IdType == "Passport")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(2);

                        }

                        _contactEntity["tra_nationality"] = new EntityReference("tra_nationality", Guid.Parse(user.nationality));


                        _contactEntity["birthdate"] = user.dateOfbirth;
                        _contactEntity.Attributes.Add("tra_title", new EntityReference("tra_title", user.titleId));
                        _contactEntity["tra_consumertype"] = new OptionSetValue(1);
                        _contactEntity["tra_countrycode"] = user.countryCode;
                        _contactEntity["mobilephone"] = user.contact;

                        if (user.socialProvider != null && user.socialProvider != string.Empty)
                        {
                            if (user.socialProvider == "Google" || user.socialProvider == "Facebook")
                            {
                                if (user.socialProvider == "Google")
                                {
                                    _contactEntity["tra_googletoken"] = user.socialProviderKey;
                                }
                                else if (user.socialProvider == "Facebook")
                                {
                                    _contactEntity["tra_facebooktoken"] = user.socialProviderKey;
                                }
                            }
                        }
                        else
                        {
                            //Changes By shweta
                            string Pass = Encryption.Encrypt(user.password);
                            _contactEntity.Attributes.Add("tra_password", Pass);//Changes
                            _contactEntity.Attributes.Add("tra_iscreatedfromportal", true);
                        }
                        

                        _contactEntity["tra_idnumber"] = user.CprPassportNo;

                        Guid contactId = service.Create(_contactEntity);

                        if (contactId != Guid.Empty)
                        {

                            consumers.errorCode = HttpStatusCode.OK;
                            consumers.message = "Indivisual user created successfully";

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>consumers);
        }
        public async Task<consumerSuccess> AddBuisnessUser(TraUser buisnessuser)
        {
            consumerSuccess consumers = new consumerSuccess();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {

                    if (buisnessuser != null)
                    {

                        Entity _contactEntity = new Entity("contact");

                        _contactEntity["firstname"] = buisnessuser.firstname.Trim();
                        _contactEntity["tra_source"] = new OptionSetValue(1);
                        _contactEntity["lastname"] = buisnessuser.lastname.Trim();

                        if (buisnessuser.emailId.Trim() != "")
                        {
                            QueryExpression _emailExist = new QueryExpression("contact");
                            _emailExist.ColumnSet = new ColumnSet("emailaddress1");
                            FilterExpression _emailFilters = new FilterExpression(LogicalOperator.And);
                            _emailFilters.AddCondition("emailaddress1", ConditionOperator.Equal, buisnessuser.emailId.Trim());
                            _emailExist.Criteria = _emailFilters;
                            EntityCollection _emailsColl = service.RetrieveMultiple(_emailExist);
                            if (_emailsColl.Entities != null && _emailsColl.Entities.Count > 0)
                            {
                                consumers.errorCode = HttpStatusCode.NotAcceptable;
                                consumers.message = "Email Exist";
                                return consumers;

                            }
                            else
                            {
                                _contactEntity["emailaddress1"] = buisnessuser.emailId.Trim();
                            }

                        }

                        if (buisnessuser.IdType == "CPR Number")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(1);

                        }
                        else if (buisnessuser.IdType == "Passport")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(2);

                        }

                        _contactEntity["tra_nationality"] = new EntityReference("tra_nationality", Guid.Parse(buisnessuser.nationality));

                        _contactEntity.Attributes.Add("tra_title", new EntityReference("tra_title", buisnessuser.titleId));
                        _contactEntity["birthdate"] = buisnessuser.dateOfbirth;
                        _contactEntity["tra_consumertype"] = new OptionSetValue(2);
                        _contactEntity["tra_countrycode"] = buisnessuser.countryCode;
                        _contactEntity["mobilephone"] = buisnessuser.contact.Trim();
                        _contactEntity["tra_companyname"] = buisnessuser.companyName.Trim();
                        _contactEntity["tra_crnumber"] = buisnessuser.commercialNo.Trim();

                        _contactEntity["tra_idnumber"] = buisnessuser.CprPassportNo.Trim();

                        if (buisnessuser.socialProvider != null && buisnessuser.socialProvider != string.Empty)
                        {
                                if (buisnessuser.socialProvider == "Google" || buisnessuser.socialProvider == "Facebook")
                                {
                                    if (buisnessuser.socialProvider == "Google")
                                    {
                                        _contactEntity["tra_googletoken"] = buisnessuser.socialProviderKey;
                                    }
                                    else if (buisnessuser.socialProvider == "Facebook")
                                    {
                                        _contactEntity["tra_facebooktoken"] = buisnessuser.socialProviderKey;
                                    }
                                }
                        }
                       else
                       {
                            //Changes By shweta
                            string BusinessUserPass = Encryption.Encrypt(buisnessuser.password);
                            _contactEntity.Attributes.Add("tra_password", BusinessUserPass);//Changes
                                _contactEntity.Attributes.Add("tra_iscreatedfromportal", true);
                       }

                        
                        Guid contactId = service.Create(_contactEntity);

                        if (contactId != Guid.Empty)
                        {

                            consumers.errorCode = HttpStatusCode.OK;
                            consumers.message = "Buisness user created successfully";

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>consumers);
        }

        public async Task<consumerSuccess> AddgovernmentUser(TraUser governmentuser)
        {
            consumerSuccess consumers = new consumerSuccess();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {

                    if (governmentuser != null)
                    {

                        Entity _contactEntity = new Entity("contact");

                        _contactEntity["firstname"] = governmentuser.firstname.Trim();
                        _contactEntity["lastname"] = governmentuser.lastname.Trim();

                      
                        if (governmentuser.emailId.Trim() != "")
                        {
                            QueryExpression _emailExist = new QueryExpression("contact");
                            _emailExist.ColumnSet = new ColumnSet("emailaddress1");
                            FilterExpression _emailFilters = new FilterExpression(LogicalOperator.And);
                            _emailFilters.AddCondition("emailaddress1", ConditionOperator.Equal, governmentuser.emailId.Trim());
                            _emailExist.Criteria = _emailFilters;
                            EntityCollection _emailsColl = service.RetrieveMultiple(_emailExist);
                            if (_emailsColl.Entities != null && _emailsColl.Entities.Count > 0)
                            {
                                consumers.errorCode = HttpStatusCode.NotAcceptable;
                                consumers.message = "Email Exist";
                                return consumers;

                            }
                            else
                            {
                                _contactEntity["emailaddress1"] = governmentuser.emailId.Trim();
                            }

                        }

                        if (governmentuser.IdType.Trim() == "CPR Number")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(1);

                        }
                        else if (governmentuser.IdType.Trim() == "Passport")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(2);

                        }

                        _contactEntity["tra_nationality"] = new EntityReference("tra_nationality", Guid.Parse(governmentuser.nationality));

                     
                        _contactEntity["birthdate"] = governmentuser.dateOfbirth;
                        _contactEntity["tra_consumertype"] = new OptionSetValue(3);
                        _contactEntity["tra_countrycode"] = governmentuser.countryCode.Trim();
                        _contactEntity["mobilephone"] = governmentuser.contact.Trim();
                        _contactEntity["tra_companyname"] = governmentuser.governmentEntity.Trim();
                        _contactEntity.Attributes.Add("tra_title", new EntityReference("tra_title", governmentuser.titleId));
                        _contactEntity["tra_idnumber"] = governmentuser.CprPassportNo.Trim();
                        _contactEntity["tra_source"] = new OptionSetValue(1);

                        if (governmentuser.socialProvider != null && governmentuser.socialProvider != string.Empty)
                        {
                            if (governmentuser.socialProvider == "Google" || governmentuser.socialProvider == "Facebook")
                            {
                                if (governmentuser.socialProvider == "Google")
                                {
                                    _contactEntity["tra_googletoken"] = governmentuser.socialProviderKey;
                                }
                                else if (governmentuser.socialProvider == "Facebook")
                                {
                                    _contactEntity["tra_facebooktoken"] = governmentuser.socialProviderKey;
                                }
                            }
                        }
                        else
                        {
                            //Changes By shweta
                            string GovernmentUserPass = Encryption.Encrypt(governmentuser.password);
                            _contactEntity.Attributes.Add("tra_password", GovernmentUserPass);//Changes
                            _contactEntity.Attributes.Add("tra_iscreatedfromportal", true);


                        }



                        Guid contactId = service.Create(_contactEntity);

                        if (contactId != Guid.Empty)
                        {

                            consumers.errorCode = HttpStatusCode.OK;
                            consumers.message = "Government user created successfully";

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>consumers);
        }
        public async Task<Boolean> IsEmailExist(string email)
        {
            Boolean isExist = false;

            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {
                    if (!string.IsNullOrEmpty(email))
                    {

                        QueryExpression _queryData = new QueryExpression("contact");
                        _queryData.ColumnSet = new ColumnSet("emailaddress1");
                        FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                        _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                        _filterData.AddCondition("tra_facebooktoken", ConditionOperator.Null);
                        _filterData.AddCondition("tra_googletoken", ConditionOperator.Null);
                        _filterData.AddCondition("emailaddress1", ConditionOperator.Equal, email.Trim());
                        _queryData.Criteria = _filterData;
                        EntityCollection _emailExist = service.RetrieveMultiple(_queryData);

                        if (_emailExist != null && _emailExist.Entities.Count > 0)
                        {
                            isExist = false;
                        }
                        else
                        {
                            isExist = true;
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>isExist);
        }

        public async Task<Boolean> IsCPRExist(string cprno)
        {
            Boolean isExist = false;

            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {
                    if (!string.IsNullOrEmpty(cprno))
                    {

                        QueryExpression _queryData = new QueryExpression("contact");
                        _queryData.ColumnSet = new ColumnSet("tra_idnumber");
                        FilterExpression _filterData = new FilterExpression(LogicalOperator.And);

                        _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                                         
                        _filterData.AddCondition("tra_idnumber", ConditionOperator.Equal, cprno.Trim());
                        _queryData.Criteria = _filterData;
                        EntityCollection _cpRExist = service.RetrieveMultiple(_queryData);

                        if (_cpRExist != null && _cpRExist.Entities.Count > 0)
                        {
                            isExist = false;
                        }
                        else
                        {
                            isExist = true;
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => isExist);
        }
        public async Task<contactvm> getConsumerbyId(string contactid)
        {

            contactvm _getContactInfo = new contactvm();

            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {

                    if (!string.IsNullOrEmpty(contactid))
                    {
                        QueryExpression _queryContactInfo = new QueryExpression("contact");
                        _queryContactInfo.ColumnSet = new ColumnSet("emailaddress1", "fullname", "tra_idtype", "tra_idnumber", "tra_nationality", "mobilephone", "birthdate", "tra_countrycode","firstname","lastname");
                        FilterExpression _filterContactData = new FilterExpression(LogicalOperator.And);
                        _filterContactData.AddCondition("statecode", ConditionOperator.Equal, 0);
                        _filterContactData.AddCondition("contactid", ConditionOperator.Equal, Guid.Parse(contactid));
                        _queryContactInfo.Criteria = _filterContactData;
                        EntityCollection _contactCollection = service.RetrieveMultiple(_queryContactInfo);

                        if (_contactCollection != null && _contactCollection.Entities.Count > 0)
                        {
                            _getContactInfo.fullnm = _contactCollection.Entities[0].Contains("fullname") ? (string)_contactCollection.Entities[0].Attributes["fullname"] : string.Empty;
                            _getContactInfo.nationality = _contactCollection.Entities[0].Contains("tra_nationality") ? (string)((EntityReference)_contactCollection.Entities[0].Attributes["tra_nationality"]).Name : null;
                            _getContactInfo.nationalityId = _contactCollection.Entities[0].Contains("tra_nationality") ? (Guid)((EntityReference)_contactCollection.Entities[0].Attributes["tra_nationality"]).Id : Guid.Empty;
                            _getContactInfo.idnumber = _contactCollection.Entities[0].Contains("tra_idnumber") ? _contactCollection.Entities[0].Attributes["tra_idnumber"].ToString() : string.Empty;
                            _getContactInfo.createdon = _contactCollection.Entities[0].Contains("birthdate") ? _contactCollection.Entities[0].Attributes["birthdate"].ToString() : string.Empty;
                            _getContactInfo.countrycode = _contactCollection.Entities[0].Contains("tra_countrycode") ? _contactCollection.Entities[0].Attributes["tra_countrycode"].ToString() : string.Empty;
                            _getContactInfo.id_type = _contactCollection.Entities[0].Contains("tra_idtype") ? _contactCollection.Entities[0].FormattedValues["tra_idtype"].ToString() : string.Empty;
                            _getContactInfo.emailaddress = _contactCollection.Entities[0].Contains("emailaddress1") ? _contactCollection.Entities[0].Attributes["emailaddress1"].ToString() : string.Empty;
                            _getContactInfo.contactid = (Guid)_contactCollection.Entities[0].Attributes["contactid"];
                            _getContactInfo.contactno = _contactCollection.Entities[0].Contains("mobilephone") ? _contactCollection.Entities[0].Attributes["mobilephone"].ToString() : string.Empty;
                            _getContactInfo.firstname = _contactCollection.Entities[0].Contains("fullname") ? (string)_contactCollection.Entities[0].Attributes["firstname"] : string.Empty;
                            _getContactInfo.lastname = _contactCollection.Entities[0].Contains("lastname") ? (string)_contactCollection.Entities[0].Attributes["lastname"] : string.Empty;
                        }


                    }

                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>_getContactInfo);
        }
        public async Task<consumerSuccess> updateUser(TraUser _updateuser)
        {
            consumerSuccess consumersUpdate = new consumerSuccess();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {
                    Entity _contactUpdates = new Entity("contact", _updateuser.ContactId);
                    if (_updateuser != null)
                    {

                        _contactUpdates["firstname"] = _updateuser.firstname.Trim();
                        _contactUpdates["lastname"] = _updateuser.lastname.Trim();
                        _contactUpdates["tra_nationality"] = new EntityReference("tra_nationality", _updateuser.nationId);
                        _contactUpdates["tra_countrycode"] = _updateuser.countryCode.Trim();
                        _contactUpdates["tra_idnumber"] = _updateuser.CprPassportNo;
                        _contactUpdates["birthdate"] = _updateuser.dateOfbirth;
                        _contactUpdates["emailaddress1"] = _updateuser.emailId;
                        _contactUpdates["mobilephone"] = _updateuser.contact.Trim();

                        if (_updateuser.IdType == "CPR Number")
                        {
                            _contactUpdates["tra_idtype"] = new OptionSetValue(1);
                        }
                        else if (_updateuser.IdType == "Passport Number")
                        {
                            _contactUpdates["tra_idtype"] = new OptionSetValue(2);
                        }

                    }

                    service.Update(_contactUpdates);

                    consumersUpdate.fullname = _updateuser.firstname + " " + _updateuser.lastname;       
                    consumersUpdate.contactId = _updateuser.ContactId;
                    consumersUpdate.firstname = _updateuser.firstname;
                    consumersUpdate.errorCode = HttpStatusCode.OK;
                    consumersUpdate.message = "Contact successfully updated";

                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>consumersUpdate);
        }
       public async Task<consumerSuccess> passwordChangeUser(changepasswordVM changepassword)
       {
            consumerSuccess _changePasswordInfo = new consumerSuccess();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {
                    if (changepassword != null)
                    {

                        QueryExpression _queryChangePass = new QueryExpression("contact");
                        _queryChangePass.ColumnSet = new ColumnSet("tra_password");
                        FilterExpression _filterpass = new FilterExpression(LogicalOperator.And);
                        _filterpass.AddCondition("statecode", ConditionOperator.Equal, 0);
                        _filterpass.AddCondition("contactid", ConditionOperator.Equal, changepassword.contactid);
                        _queryChangePass.Criteria = _filterpass;
                        EntityCollection _contactCollection = service.RetrieveMultiple(_queryChangePass);

                        if (_contactCollection != null && _contactCollection.Entities.Count > 0)
                        {
                            string tra_password = _contactCollection.Entities[0].Attributes["tra_password"].ToString();
                            
                            //Changes By Shweta
                            string Password = Encryption.Decrypt(tra_password);

                            if (Password == changepassword.currentPassword.Trim())//Changes
                            {
                                Entity _changecontactPassword = new Entity("contact", changepassword.contactid);

                                //Changes By Shweta
                                String ChangenewPassword = Encryption.Encrypt(changepassword.newPassword.Trim());
                                _changecontactPassword["tra_password"] = ChangenewPassword;//Changes
                                service.Update(_changecontactPassword);

                                /////////////////////////////////////////////////////
                                _changePasswordInfo.contactId = changepassword.contactid;
                                _changePasswordInfo.errorCode = HttpStatusCode.OK;
                                _changePasswordInfo.message = "User password changed successfully";
                            }
                            else
                            {
                                _changePasswordInfo.errorCode = HttpStatusCode.NotFound;
                                _changePasswordInfo.message = "Current Password Wrong";

                            }

                        }

                    }

                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>_changePasswordInfo);
        }

        public UserInfoViewModel getGoogleFacebookUserInfo(userModel externallogininfo)
        {
            UserInfoViewModel userInfoView = new UserInfoViewModel();
            try
            {

                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {

                    if (externallogininfo.LoginProvider == "Google")
                    {

                        QueryExpression _queryData = new QueryExpression("contact");
                        _queryData.ColumnSet = new ColumnSet("emailaddress1", "tra_googletoken", "fullname", "firstname");
                        FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                        _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                        _filterData.AddCondition("emailaddress1", ConditionOperator.Equal, externallogininfo.Email);
                        _filterData.AddCondition("tra_googletoken", ConditionOperator.Equal, externallogininfo.ProviderKey);
                        _queryData.Criteria = _filterData;
                        EntityCollection userExist = service.RetrieveMultiple(_queryData);

                        if (userExist != null && userExist.Entities != null && userExist.Entities.Count > 0)
                        {
                            userInfoView.Email = userExist.Entities[0].Contains("emailaddress1") ? (string)userExist.Entities[0].Attributes["emailaddress1"] : string.Empty;
                            userInfoView.contactId = (Guid)userExist.Entities[0].Attributes["contactid"];
                            userInfoView.fullname = userExist.Entities[0].Contains("fullname") ? (string)userExist.Entities[0].Attributes["fullname"] : string.Empty;
                            userInfoView.HasRegistered = true;
                            userInfoView.LoginProvider = externallogininfo.LoginProvider;
                            userInfoView.providerKey = externallogininfo.ProviderKey;
                            userInfoView.firstname = userExist.Entities[0].Contains("firstname") ? (string)userExist.Entities[0].Attributes["firstname"] : string.Empty;

                        }
                        else
                        {
                            QueryExpression _chkgooglequeryData = new QueryExpression("contact");
                            _chkgooglequeryData.ColumnSet = new ColumnSet("emailaddress1", "tra_googletoken", "fullname", "firstname");
                            FilterExpression _googlefilterData = new FilterExpression(LogicalOperator.And);
                            _googlefilterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                            _googlefilterData.AddCondition("emailaddress1", ConditionOperator.Equal, externallogininfo.Email);
                            _chkgooglequeryData.Criteria = _googlefilterData;
                            EntityCollection googleuserExist = service.RetrieveMultiple(_chkgooglequeryData);
                            if (googleuserExist != null && googleuserExist.Entities != null && googleuserExist.Entities.Count > 0)
                            {
                                Entity user = new Entity("contact");
                                user.Attributes["tra_googletoken"] = externallogininfo.ProviderKey;
                                user.Id = googleuserExist.Entities[0].Id;
                                service.Update(user);
                                userInfoView.Email = googleuserExist.Entities[0].Contains("emailaddress1") ? (string)googleuserExist.Entities[0].Attributes["emailaddress1"] : string.Empty;
                                userInfoView.contactId = (Guid)googleuserExist.Entities[0].Attributes["contactid"];
                                userInfoView.fullname = googleuserExist.Entities[0].Contains("fullname") ? (string)googleuserExist.Entities[0].Attributes["fullname"] : string.Empty;
                                userInfoView.HasRegistered = true;
                                userInfoView.LoginProvider = externallogininfo.LoginProvider;
                                userInfoView.providerKey = externallogininfo.ProviderKey;
                                userInfoView.firstname = googleuserExist.Entities[0].Contains("firstname") ? (string)googleuserExist.Entities[0].Attributes["firstname"] : string.Empty;
                            }
                            else
                            {
                                userInfoView.HasRegistered = false;
                                userInfoView.LoginProvider = externallogininfo.LoginProvider;
                                userInfoView.Email = externallogininfo.Email;
                                userInfoView.providerKey = externallogininfo.ProviderKey;
                                userInfoView.fullname = externallogininfo.fullname;
                            }
                        }
                    }
                    else if (externallogininfo.LoginProvider == "Facebook")
                    {
                        QueryExpression _queryData = new QueryExpression("contact");
                        _queryData.ColumnSet = new ColumnSet("emailaddress1", "tra_facebooktoken", "fullname", "firstname");
                        FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                        _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                        _filterData.AddCondition("emailaddress1", ConditionOperator.Equal, externallogininfo.Email);
                        _filterData.AddCondition("tra_facebooktoken", ConditionOperator.Equal, externallogininfo.ProviderKey);
                        _queryData.Criteria = _filterData;
                        EntityCollection userExist = service.RetrieveMultiple(_queryData);

                        if (userExist != null && userExist.Entities != null && userExist.Entities.Count > 0)
                        {
                            userInfoView.Email = userExist.Entities[0].Contains("emailaddress1") ? (string)userExist.Entities[0].Attributes["emailaddress1"] : string.Empty;
                            userInfoView.contactId = (Guid)userExist.Entities[0].Attributes["contactid"];
                            userInfoView.fullname = userExist.Entities[0].Contains("fullname") ? (string)userExist.Entities[0].Attributes["fullname"] : string.Empty;
                            userInfoView.HasRegistered = true;
                            userInfoView.LoginProvider = externallogininfo.LoginProvider;
                            userInfoView.firstname = userExist.Entities[0].Contains("firstname") ? (string)userExist.Entities[0].Attributes["firstname"] : string.Empty;
                            userInfoView.providerKey = externallogininfo.ProviderKey;

                        }
                        else
                        {
                            QueryExpression _fbqueryData = new QueryExpression("contact");
                            _fbqueryData.ColumnSet = new ColumnSet("emailaddress1", "tra_facebooktoken", "fullname", "firstname");
                            FilterExpression _fbfilterData = new FilterExpression(LogicalOperator.And);
                            _fbfilterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                            _fbfilterData.AddCondition("emailaddress1", ConditionOperator.Equal, externallogininfo.Email);
                            _fbqueryData.Criteria = _fbfilterData;
                            EntityCollection fbuserExist = service.RetrieveMultiple(_fbqueryData);
                            if (fbuserExist != null && fbuserExist.Entities != null && fbuserExist.Entities.Count > 0)
                            {
                                Entity user = new Entity("contact");
                                user.Attributes["tra_facebooktoken"] = externallogininfo.ProviderKey;
                                user.Id = fbuserExist.Entities[0].Id;
                                service.Update(user);
                                userInfoView.Email = fbuserExist.Entities[0].Contains("emailaddress1") ? (string)fbuserExist.Entities[0].Attributes["emailaddress1"] : string.Empty;
                                userInfoView.contactId = (Guid)fbuserExist.Entities[0].Attributes["contactid"];
                                userInfoView.fullname = fbuserExist.Entities[0].Contains("fullname") ? (string)fbuserExist.Entities[0].Attributes["fullname"] : string.Empty;
                                userInfoView.HasRegistered = true;
                                userInfoView.LoginProvider = externallogininfo.LoginProvider;
                                userInfoView.providerKey = externallogininfo.ProviderKey;
                                userInfoView.firstname = fbuserExist.Entities[0].Contains("firstname") ? (string)fbuserExist.Entities[0].Attributes["firstname"] : string.Empty;
                            }
                            else
                            {
                                userInfoView.HasRegistered = false;
                                userInfoView.LoginProvider = externallogininfo.LoginProvider;
                                userInfoView.Email = externallogininfo.Email;
                                userInfoView.providerKey = externallogininfo.ProviderKey;
                                userInfoView.fullname = externallogininfo.fullname;
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }

            return userInfoView;
        }

        //public UserInfoViewModel getGoogleFacebookUserInfo(userModel externallogininfo)
        //{
        //    UserInfoViewModel userInfoView = new UserInfoViewModel();
        //    try
        //    {

        //        OrganizationService organization = new OrganizationService();
        //        ConfigData configData = ConfigEncrypt.GetCrmCredentials();
        //        IOrganizationService service = organization.GetCRMService(configData);

        //        if (service != null)
        //        {

        //            if (externallogininfo.LoginProvider == "Google")
        //            {

        //                QueryExpression _queryData = new QueryExpression("contact");
        //                _queryData.ColumnSet = new ColumnSet("emailaddress1", "tra_googletoken", "fullname","firstname");
        //                FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
        //                _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
        //                _filterData.AddCondition("emailaddress1", ConditionOperator.Equal, externallogininfo.Email);
        //                _filterData.AddCondition("tra_googletoken", ConditionOperator.Equal, externallogininfo.ProviderKey);
        //                _queryData.Criteria = _filterData;
        //                EntityCollection userExist = service.RetrieveMultiple(_queryData);

        //                if (userExist != null && userExist.Entities != null && userExist.Entities.Count > 0)
        //                {

        //                    userInfoView.Email = userExist.Entities[0].Contains("emailaddress1") ? (string)userExist.Entities[0].Attributes["emailaddress1"] : string.Empty;
        //                    userInfoView.contactId = (Guid)userExist.Entities[0].Attributes["contactid"];
        //                     userInfoView.fullname = userExist.Entities[0].Contains("fullname") ? (string)userExist.Entities[0].Attributes["fullname"] : string.Empty;
        //                    userInfoView.HasRegistered = true;
        //                    userInfoView.LoginProvider = externallogininfo.LoginProvider;                        
        //                    userInfoView.providerKey = externallogininfo.ProviderKey;
        //                    userInfoView.firstname= userExist.Entities[0].Contains("firstname") ? (string)userExist.Entities[0].Attributes["firstname"] : string.Empty;

        //                }
        //                else
        //                {

        //                    userInfoView.HasRegistered = false;
        //                    userInfoView.LoginProvider = externallogininfo.LoginProvider;
        //                    userInfoView.fullname = externallogininfo.fullname;
        //                    userInfoView.Email = externallogininfo.Email;
        //                    userInfoView.providerKey = externallogininfo.ProviderKey;
        //                }
        //            }
        //            else if (externallogininfo.LoginProvider == "Facebook")
        //            {
        //                QueryExpression _queryData = new QueryExpression("contact");
        //                _queryData.ColumnSet = new ColumnSet("emailaddress1", "tra_facebooktoken", "fullname","firstname");
        //                FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
        //                _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
        //                _filterData.AddCondition("emailaddress1", ConditionOperator.Equal, externallogininfo.Email);
        //                _filterData.AddCondition("tra_facebooktoken", ConditionOperator.Equal, externallogininfo.ProviderKey);
        //                _queryData.Criteria = _filterData;
        //                EntityCollection userExist = service.RetrieveMultiple(_queryData);

        //                if (userExist != null && userExist.Entities != null && userExist.Entities.Count > 0)
        //                {


        //                    userInfoView.Email = userExist.Entities[0].Contains("emailaddress1") ? (string)userExist.Entities[0].Attributes["emailaddress1"] : string.Empty;
        //                    userInfoView.contactId = (Guid)userExist.Entities[0].Attributes["contactid"];
        //                    userInfoView.fullname = userExist.Entities[0].Contains("fullname") ? (string)userExist.Entities[0].Attributes["fullname"] : string.Empty;
        //                    userInfoView.HasRegistered = true;
        //                    userInfoView.LoginProvider = externallogininfo.LoginProvider;
        //                    userInfoView.firstname = userExist.Entities[0].Contains("firstname") ? (string)userExist.Entities[0].Attributes["firstname"] : string.Empty;

        //                    userInfoView.providerKey = externallogininfo.ProviderKey;


        //                }
        //                else
        //                {

        //                    userInfoView.HasRegistered = false;
        //                    userInfoView.LoginProvider = externallogininfo.LoginProvider;
        //                    userInfoView.Email = externallogininfo.Email;
        //                    userInfoView.providerKey = externallogininfo.ProviderKey;
        //                    userInfoView.fullname = externallogininfo.fullname;

        //                }


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Exceptions.exceptionHandler(ex.ToString());
        //        throw ex;
        //    }

        //    return userInfoView;
        //}



        public async Task<caseDetail> userComment()
       {
            caseDetail _consumerReply = new caseDetail();
            try
            {
                //documentsData _documentsReqData = null;
                List<documentsData> listdocReq = new List<documentsData>();
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {
                    var httpRequestData = HttpContext.Current.Request;
                    if (httpRequestData.Files.Count > 0)
                    {
                        //foreach (string filesItem in httpRequestData.Files)
                        //{
                        //    _documentsReqData = new documentsData();
                        //    HttpPostedFile httpPostedFiles = httpRequestData.Files[filesItem];
                        //    Stream fs = httpPostedFiles.InputStream;
                        //    BinaryReader br = new System.IO.BinaryReader(fs);
                        //    Byte[] fileBytes = br.ReadBytes((Int32)fs.Length);
                        //    _documentsReqData.documents = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                        //    listdocReq.Add(_documentsReqData);
                        //    IntPtr mimeTypePtr;
                        //    FindMimeFromData(IntPtr.Zero, null, fileBytes, 256, null, 0, out mimeTypePtr, 0);
                        //    string mime = Marshal.PtrToStringUni(mimeTypePtr);
                        //    Marshal.FreeCoTaskMem(mimeTypePtr);

                        //    if (!mappings.Contains(mime))
                        //    {
                        //        _consumerReply.errorCode = HttpStatusCode.BadRequest;
                        //        return _consumerReply;
                        //    }


                        //}

                    }

                    

                    if (httpRequestData.Form.Count > 0)
                    {
                        replyModel _replyModel = new replyModel();
                        _replyModel.caseId =Guid.Parse(httpRequestData["caseId"]);
                        

                        if (_replyModel != null)
                        {
                            Entity _conversionEntity = new Entity("tra_conversation");

                            _conversionEntity["tra_caseid"] = new EntityReference("incident", _replyModel.caseId);
                            _conversionEntity["tra_comment"] = "Other Supporting Documents (if any)";
                            _conversionEntity.Attributes.Add("tra_commentby", new OptionSetValue(1));
                            _conversionEntity["tra_consumer"] = Convert.ToBoolean(true);
                            _conversionEntity["tra_showonlyattachmentsonportal"] = Convert.ToBoolean(true);
                            Guid conversationId = service.Create(_conversionEntity);

                            if (conversationId != Guid.Empty)
                            {

                                List<documentBody> lystdocumentBody = new List<documentBody>();
                                List<postedFile> listPostedFiles = new List<postedFile>();
                                List<Entity> AnnotationEntityList = new List<Entity>();
                                postedFile postedFiles = null;
                                documentBody _documentuploads = null;

                              

                                if (httpRequestData.Files.Count > 0)
                                {
                                    foreach (string file in httpRequestData.Files)
                                    {
                                        postedFiles = new postedFile();
                                        postedFiles.httppostedFile = httpRequestData.Files[file];
                                        postedFiles.filename = postedFiles.httppostedFile.FileName;
                                        listPostedFiles.Add(postedFiles);

                                    }

                                    if (listPostedFiles != null&&listPostedFiles.Count>0)
                                    {
                                        foreach (var items in listPostedFiles)
                                        {
                                            _documentuploads = new documentBody();
                                            Stream fs = items.httppostedFile.InputStream;
                                            BinaryReader br = new System.IO.BinaryReader(fs);
                                            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                            _documentuploads.documentbody = Convert.ToBase64String(bytes, 0, bytes.Length);
                                            _documentuploads.filename = items.filename;
                                            lystdocumentBody.Add(_documentuploads);
                                        }

                                        //if (listdocReq != null && listdocReq.Count > 0)
                                        //{
                                        //    for(int u = 0; u < listdocReq.Count; u++)
                                        //    {
                                        //        lystdocumentBody[u].documentbody = listdocReq[u].documents;

                                        //    }
                                        //}

                                        if (lystdocumentBody != null && lystdocumentBody.Count > 0)
                                        {
                                            for (int i = 0; i < lystdocumentBody.Count; i++)
                                            {
                                                Entity annotationEntity = new Entity("annotation");
                                                annotationEntity["documentbody"] = lystdocumentBody[i].documentbody;
                                                annotationEntity["filename"] = lystdocumentBody[i].filename;
                                                AnnotationEntityList.Add(annotationEntity);

                                            }


                                            if (AnnotationEntityList != null && AnnotationEntityList.Count > 0)
                                            {

                                                int m = 0;
                                                foreach (var dataAttachment in AnnotationEntityList)
                                                {
                                                    AnnotationEntityList[m].Attributes.Add("objectid", new EntityReference("tra_conversation", conversationId));
                                                    service.Create(AnnotationEntityList[m]);
                                                    m++;
                                                }

                                                _consumerReply.errorCode = HttpStatusCode.OK;
                                                _consumerReply.caseNo = _replyModel.caseNo;
                                                _consumerReply.message = "Conversation created with Attachment";
                                                return await Task.Run(() => _consumerReply);

                                            }

                                        }

                                    }
                                }

                                _consumerReply.errorCode = HttpStatusCode.OK;
                                _consumerReply.caseNo = _replyModel.caseNo;
                                _consumerReply.message = "Conversation created without Attachment";


                            }

                        }


                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return await Task.Run(() => _consumerReply);
       }

        public async Task<Boolean> IsPassCheck(string password, string contactid)
        {
            Boolean isExist = false;

            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {
                    if (!string.IsNullOrEmpty(password)&& !string.IsNullOrEmpty(contactid))
                    {
                        Guid contact = Guid.Parse(contactid);
                        QueryExpression _queryData = new QueryExpression("contact");
                        _queryData.ColumnSet = new ColumnSet("tra_password");
                        FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                        
                        _filterData.AddCondition("contactid", ConditionOperator.Equal, contact);
                        _queryData.Criteria = _filterData;
                        EntityCollection passExist = service.RetrieveMultiple(_queryData);

                        if (passExist != null && passExist.Entities.Count > 0)
                        {
                            string passwords = passExist.Entities[0].Attributes["tra_password"].ToString();

                            //Changes By Shweta
                            String ExistPassword = Encryption.Decrypt(passwords);

                            if (password.Trim() == ExistPassword)//Changees
                            {
                                isExist = true;
                            }
                            else
                            {
                                isExist = false;
                            }
                            
                        }
                      


                    }
                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => isExist);
        }

        public async Task<surveyDetails> surveyDetails(string caseId)
        {
            Boolean IsData = false;
            surveyDetails survey = new surveyDetails();
            try
            {
                if (!string.IsNullOrEmpty(caseId))
                {
                    OrganizationService organization = new OrganizationService();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    IOrganizationService service = organization.GetCRMService(configData);

                    if (service != null)
                    {
                        QueryExpression _serveyData = new QueryExpression("tra_survey");
                        _serveyData.ColumnSet = new ColumnSet("tra_name", "createdon");
                        FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                        _filterData.AddCondition("tra_case", ConditionOperator.Equal, Guid.Parse(caseId.Trim()));
                        _serveyData.Criteria = _filterData;
                        EntityCollection entity = service.RetrieveMultiple(_serveyData);
                        if (entity != null && entity.Entities.Count > 0)
                        {
                            IsData= false;
                            survey.isSurvey = IsData;
                            return await Task.Run(() => survey);
                        }
                        else
                        {
                            IsData = true;
                            survey.isSurvey = IsData;
                        }


                        QueryExpression _caseQuery = new QueryExpression("incident");
                        _caseQuery.ColumnSet = new ColumnSet("ticketnumber");
                        FilterExpression _caseFilter = new FilterExpression(LogicalOperator.And);
                        _caseFilter.AddCondition("incidentid", ConditionOperator.Equal, Guid.Parse(caseId));
                        _caseQuery.Criteria = _caseFilter;
                        EntityCollection _caseNo = service.RetrieveMultiple(_caseQuery);
                        if (_caseNo != null && _caseNo.Entities.Count > 0)
                        {
                            survey.ticketNo = _caseNo.Entities[0].Attributes["ticketnumber"].ToString();

                        }

                       
                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => survey);
        }

        public async Task<surveyDetails> postSurvey(surveyData survey)
        {
            surveyDetails _surveyinfo = new surveyDetails();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {


                    if (survey != null)
                    {
                        Entity surveyEntity = new Entity("tra_survey");
                        if (survey.survey1 != null)
                        {
                            int value = getOptionSetValue(survey.survey1, service, "tra_surveyquestion1");
                            surveyEntity.Attributes.Add("tra_surveyquestion1", new OptionSetValue(value));
                        }
                        if (survey.survey2 != null)
                        {
                            int value1 = getOptionSetValue(survey.survey2, service, "tra_surveyquestion2");
                            surveyEntity.Attributes.Add("tra_surveyquestion2", new OptionSetValue(value1));
                        }
                        if (survey.survey3 != null)
                        {
                            int value2 = getOptionSetValue(survey.survey3, service, "tra_surveyquestion3");
                            surveyEntity.Attributes.Add("tra_surveyquestion3", new OptionSetValue(value2));
                        }
                        if (survey.survey4 != null)
                        {
                            int value3 = getOptionSetValue(survey.survey4, service, "tra_surveyquestion4");
                            surveyEntity.Attributes.Add("tra_surveyquestion4", new OptionSetValue(value3));
                        }

                        if (survey.comments != "" && survey.comments != null)
                        {
                            surveyEntity.Attributes.Add("tra_surveyquestion5", survey.comments);
                        }

                        surveyEntity.Attributes.Add("tra_case", new EntityReference("incident", survey.caseid));

                        Guid Id = service.Create(surveyEntity);

                        if (Id != Guid.Empty)
                        {
                            _surveyinfo.errorCode = HttpStatusCode.OK;
                     
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => _surveyinfo);

        }

        public async Task<caseInformation> getConsumerConsentInfo(string caseid)
        { 
       
            caseInformation _caseInfo = new caseInformation();
            caseDetails _caseData = null;
            conversationView _conversationView = null;
            annotationClass annotation =null;
            List<conversationView> _listConversation = new List<conversationView>();
            Boolean IsConsent = false;
            List<annotationClass> annotationColl = new List<annotationClass>();
            try
            {
                if (!string.IsNullOrEmpty(caseid))
                {
                    OrganizationService organization = new OrganizationService();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    IOrganizationService service = organization.GetCRMService(configData);

                    if (service != null)
                    {


                        string ConsentXML = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                              <entity name='incident'>
                                              <attribute name='title' />
                                               <attribute name='ticketnumber' />
                                              <attribute name='createdon' />
                                               <attribute name='incidentid' />
                                                    <attribute name='caseorigincode' />
                                          <order attribute='title' descending='false' />
                                             <filter type='and'>
                                          <condition attribute='tra_consentstatus' operator='eq' value='1' />
                                          <condition attribute='incidentid' operator='eq'  value='" + caseid + @"' />
                                             </filter>
                                             </entity>
                                                    </fetch>";

                        Entity ConsentCancel = service.Retrieve("incident",Guid.Parse(caseid.Trim()), new ColumnSet(true));

                        FetchExpression fetch = new FetchExpression(ConsentXML);
                        EntityCollection Consent = service.RetrieveMultiple(fetch);
                        if (Consent.Entities.Count > 0 || ((OptionSetValue)ConsentCancel["statuscode"]).Value == 167490021)
                        {
                            _caseInfo._IsConsent = IsConsent;

                        }
                        else
                        {
                            IsConsent = true;
                            _caseInfo._IsConsent = IsConsent;
                            QueryExpression _queryData = new QueryExpression("incident");
                            _queryData.ColumnSet = new ColumnSet(new string[]
                            {
                               "ticketnumber","createdon","casetypecode","statecode","tra_casequestion1","tra_casestatusforconsumer","tra_service","tra_complainttype","tra_dateofcomplaintagainstserviceprovider",
                                 "customerid","tra_disputednumber","tra_serviceprovider","tra_casequestion3","tra_casequestion2","tra_serviceprovidercasereference","description" });


                            FilterExpression filterData = new FilterExpression(LogicalOperator.And);
                            filterData.AddCondition("incidentid", ConditionOperator.Equal, Guid.Parse(caseid));

                            _queryData.Criteria = filterData;
                            EntityCollection _entity = service.RetrieveMultiple(_queryData);

                            if (_entity != null && _entity.Entities.Count > 0)
                            {

                                _caseData = new caseDetails();
                               
                                
                                _caseData.caseNo = _entity.Entities[0].Contains("ticketnumber") ? (string)_entity.Entities[0].Attributes["ticketnumber"] : string.Empty;
                                _caseData.complaintType = _entity.Entities[0].Contains("tra_complainttype") ? (string)((EntityReference)_entity.Entities[0].Attributes["tra_complainttype"]).Name : null;
                                _caseData.complaintId = _entity.Entities[0].Contains("tra_complainttype") ? (Guid)((EntityReference)_entity.Entities[0].Attributes["tra_complainttype"]).Id : Guid.Empty;
                                _caseData.service = _entity.Entities[0].Contains("tra_service") ? (string)((EntityReference)_entity.Entities[0].Attributes["tra_service"]).Name : null;
                                _caseData.serviceId = _entity.Entities[0].Contains("tra_service") ? (Guid)((EntityReference)_entity.Entities[0].Attributes["tra_service"]).Id : Guid.Empty;
                                _caseData.serviceProvider = _entity.Entities[0].Contains("tra_serviceprovider") ? (string)((EntityReference)_entity.Entities[0].Attributes["tra_serviceprovider"]).Name : null;
                                _caseData.serviceProviderId = _entity.Entities[0].Contains("tra_serviceprovider") ? (Guid)((EntityReference)_entity.Entities[0].Attributes["tra_serviceprovider"]).Id : Guid.Empty;
                                _caseData.fullname = _entity.Entities[0].Contains("customerid") ? (string)((EntityReference)_entity.Entities[0].Attributes["customerid"]).Name : null;
                                _caseData.contactid = _entity.Entities[0].Contains("customerid") ? (Guid)((EntityReference)_entity.Entities[0].Attributes["customerid"]).Id : Guid.Empty;
                                DateTime date = (DateTime)_entity.Entities[0].Attributes["createdon"];
                                _caseData.submitComplaintDate = (DateTime)_entity.Entities[0].Attributes["createdon"];
                                _caseData.createOn = date.ToString("dd/MM/yyy", CultureInfo.InvariantCulture);
                                _caseData.caseType = _entity.Entities[0].Contains("casetypecode") ? (string)_entity.Entities[0].FormattedValues["casetypecode"] : string.Empty;
                                _caseData.caseRefNo = _entity.Entities[0].Contains("tra_serviceprovidercasereference") ? (string)_entity.Entities[0].Attributes["tra_serviceprovidercasereference"] : string.Empty;
                                if (_caseData.caseType == "Complaint")
                                {
                                    if (_entity.Entities[0].Contains("tra_dateofcomplaintagainstserviceprovider")) 
                                    {
                                        DateTime? dt = _entity.Entities[0].Contains("tra_dateofcomplaintagainstserviceprovider") ? (DateTime)_entity.Entities[0].Attributes["tra_dateofcomplaintagainstserviceprovider"] : (DateTime?)null;
                                      
                                        _caseData.complaintAgainstSp = dt;
                                    }
                                    _caseData.caseDescription = _entity.Entities[0].Contains("tra_casequestion1") ? (string)_entity.Entities[0].Attributes["tra_casequestion1"] : string.Empty;
                                }else if(_caseData.caseType == "Enquiry"|| _caseData.caseType == "Suggestion")
                                {
                                    _caseData.caseDescription = _entity.Entities[0].Contains("description") ? (string)_entity.Entities[0].Attributes["description"] : string.Empty;
                                }
                                _caseData.disputNo = _entity.Entities[0].Contains("tra_disputednumber") ? (string)_entity.Entities[0].Attributes["tra_disputednumber"] : string.Empty;
                              
                                _caseData.actionTakeSp = _entity.Entities[0].Contains("tra_casequestion2") ? (string)_entity.Entities[0].Attributes["tra_casequestion2"] : string.Empty;
                                _caseData.satisfySolution = _entity.Entities[0].Contains("tra_casequestion3") ? (string)_entity.Entities[0].Attributes["tra_casequestion3"] : string.Empty;

                            }

                            if (_caseData != null)
                            {

                                QueryExpression convQuery = new QueryExpression("tra_conversation");
                                convQuery.ColumnSet = new ColumnSet("tra_commentby", "tra_comment", "createdon", "tra_showonlyattachmentsonportal");

                                FilterExpression filterConversation = new FilterExpression(LogicalOperator.And);
                                filterConversation.AddCondition("tra_consumer", ConditionOperator.Equal, true);
                                filterConversation.AddCondition("tra_caseid", ConditionOperator.Equal, Guid.Parse(caseid));
                                convQuery.Criteria = filterConversation;

                                EntityCollection _conversationCollection = service.RetrieveMultiple(convQuery);

                                if (_conversationCollection.Entities != null && _conversationCollection.Entities.Count > 0)
                                {
                                    foreach (Entity itemData in _conversationCollection.Entities)
                                    {
                                        _conversationView = new conversationView();


                                        _conversationView.comments = itemData.Contains("tra_comment") ? (string)itemData.Attributes["tra_comment"] : string.Empty;
                                        _conversationView.commentBy = itemData.Contains("tra_commentby") ? (string)itemData.FormattedValues["tra_commentby"] : string.Empty;
                                        _conversationView.createdOn = (DateTime)itemData.Attributes["createdon"];
                                        _conversationView.conversationId = (Guid)itemData.Attributes["tra_conversationid"];
                                        _conversationView.isPortal = itemData.Contains("tra_showonlyattachmentsonportal") ? (Boolean)itemData.Attributes["tra_showonlyattachmentsonportal"] : false;

                                        _listConversation.Add(_conversationView);


                                    }

                                    if (_listConversation != null && _listConversation.Count > 0)
                                    {
                                        foreach (var conversationItems in _listConversation)
                                        {

                                            if (conversationItems.commentBy == "Consumer" && conversationItems.isPortal == true)
                                            {
                                                QueryExpression annotQuery = new QueryExpression("annotation");
                                                annotQuery.ColumnSet = new ColumnSet(new string[]
                                                {
                                                    "filename"
                                                });
                                                FilterExpression _annotFilters = new FilterExpression(LogicalOperator.And);
                                                _annotFilters.AddCondition("objectid", ConditionOperator.Equal, conversationItems.conversationId);
                                                annotQuery.Criteria = _annotFilters;
                                                EntityCollection _annotationCollections = service.RetrieveMultiple(annotQuery);

                                                if (_annotationCollections.Entities != null && _annotationCollections.Entities.Count > 0)
                                                {
                                                    foreach (Entity annotItems in _annotationCollections.Entities)
                                                    {
                                                        annotation = new annotationClass();
                                                        annotation.annotId = (Guid)annotItems.Attributes["annotationid"];
                                                        annotation.filename = annotItems.Contains("filename") ? annotItems.Attributes["filename"].ToString() : string.Empty;

                                                        annotation.commentBy = conversationItems.commentBy;
                                                        annotation.IsportalAttach = conversationItems.isPortal;
                                                        annotationColl.Add(annotation);
                                                    }


                                                }

                                            }


                                        }

                                    }



                                }
                            }

                        }


                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }

            _caseInfo._caseData = _caseData;
            _caseInfo.listAnnotation = annotationColl;
            return await Task.Run(() => _caseInfo);
        }


        public async Task<consumerSuccess> postConsentData(consumerConsent _consentdata)
        {
            consumerSuccess _consentPost = new consumerSuccess();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null && _consentdata != null)
                {
                    Entity _consentEntity = new Entity();
                    _consentEntity.LogicalName = "incident";



                    if (_consentdata.contactid != Guid.Empty)
                    {
                        _consentEntity["customerid"] = new EntityReference("contact", _consentdata.contactid);
                    }
                  

                    if (_consentdata.caseType == "Complaint")
                    {
                        _consentEntity["casetypecode"] = new OptionSetValue(Convert.ToInt32(1));
                    }
                    else if (_consentdata.caseType == "Enquiry")
                    {
                        _consentEntity["casetypecode"] = new OptionSetValue(Convert.ToInt32(2));
                    }
                    else if (_consentdata.caseType == "Suggestion")
                    {
                        _consentEntity["casetypecode"] = new OptionSetValue(Convert.ToInt32(2));
                    }

                    if (_consentdata.serviceProviderId != Guid.Empty)
                    {
                        _consentEntity["tra_serviceprovider"] = new EntityReference("tra_serviceprovider", _consentdata.serviceProviderId);
                    }
                    if (_consentdata.serviceId != Guid.Empty)
                    {
                        _consentEntity["tra_service"] = new EntityReference("tra_service", _consentdata.serviceId);
                    }
                    if (_consentdata.complaintId != Guid.Empty)
                    {
                        _consentEntity["tra_complainttype"] = new EntityReference("tra_complainttype", _consentdata.complaintId);

                    }

                    if (_consentdata.caseRefNo.Trim() != "" && _consentdata.caseRefNo.Trim() != null)
                    {
                        _consentEntity["tra_serviceprovidercasereference"] = _consentdata.caseRefNo.Trim();
                    }

                    if (_consentdata.disputeNo.Trim() != string.Empty)
                    {
                        _consentEntity["tra_disputednumber"] = _consentdata.disputeNo.Trim();
                    }
                   
                    _consentEntity["ticketnumber"] = _consentdata.ticketno;

                    if(_consentdata.caseType=="Complaint"&& _consentdata.complaintDate != "")
                    {
                        _consentEntity["tra_dateofcomplaintagainstserviceprovider"] = Convert.ToDateTime(_consentdata.complaintDate);
                    }

                    if (_consentdata.question1 != "")
                    {
                        _consentEntity["tra_casequestion1"] = _consentdata.question1;
                    }
                    if (_consentdata.question2 != "")
                    {
                        _consentEntity["tra_casequestion2"] = _consentdata.question2;
                    }
                    if (_consentdata.question3 != "")
                    {
                        _consentEntity["tra_casequestion3"] = _consentdata.question3;
                    }


                    _consentEntity.Attributes.Add("tra_consentstatus", new OptionSetValue(1));
                    _consentEntity.Attributes.Add("statuscode", new OptionSetValue(167490006));
                    _consentEntity.Id = _consentdata.caseId;
                    service.Update(_consentEntity);

                }
                    
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            _consentPost.errorCode = HttpStatusCode.OK;
            return await Task.Run(() => _consentPost);
        }

        public async Task<caseInformation> getDocumentRequest(string caseid)
        {

            caseInformation _caseInfo = new caseInformation();
            caseDetails _caseData = null;
            conversationView _conversationView = null;
            annotationClass annotation = null;
            List<conversationView> _listConversation = new List<conversationView>();
            
            List<annotationClass> annotationColl = new List<annotationClass>();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null)
                {
                    if (!string.IsNullOrEmpty(caseid.Trim()))
                    {
                        QueryExpression _queryData = new QueryExpression("incident");
                        _queryData.ColumnSet = new ColumnSet("ticketnumber", "createdon", "casetypecode",  "tra_casequestion1", "tra_casestatusforconsumer","description");

                        FilterExpression filterData = new FilterExpression(LogicalOperator.And);
                        filterData.AddCondition("incidentid", ConditionOperator.Equal, Guid.Parse(caseid.Trim()));

                        _queryData.Criteria = filterData;

                        EntityCollection _entity = service.RetrieveMultiple(_queryData);

                        if (_entity != null && _entity.Entities.Count > 0)
                        {
                            _caseData = new caseDetails();

                            _caseData.caseNo = _entity.Entities[0].Contains("ticketnumber") ? (string)_entity.Entities[0].Attributes["ticketnumber"] : string.Empty;
                           
                            DateTime date = (DateTime)_entity.Entities[0].Attributes["createdon"];
                           
                            _caseData.createOn = date.ToString("dd/MM/yyy", CultureInfo.InvariantCulture);
                            _caseData.caseType = _entity.Entities[0].Contains("casetypecode") ? (string)_entity.Entities[0].FormattedValues["casetypecode"] : string.Empty;

                            if (_caseData.caseType == "Complaint")
                            {
                                _caseData.caseDescription = _entity.Entities[0].Contains("tra_casequestion1") ? (string)_entity.Entities[0].Attributes["tra_casequestion1"] : string.Empty;
                            }
                            else if (_caseData.caseType == "Enquiry"|| _caseData.caseType == "Suggestion")
                            {
                                _caseData.caseDescription = _entity.Entities[0].Contains("desciption") ? (string)_entity.Entities[0].Attributes["desciption"] : string.Empty;
                            }
                          
                        }

                        if (_caseData != null)
                        {

                            QueryExpression convQuery = new QueryExpression("tra_conversation");
                            convQuery.ColumnSet = new ColumnSet("tra_commentby", "tra_comment", "createdon", "tra_showonlyattachmentsonportal");

                            FilterExpression filterConversation = new FilterExpression(LogicalOperator.And);
                            filterConversation.AddCondition("tra_consumer", ConditionOperator.Equal, true);
                            filterConversation.AddCondition("tra_caseid", ConditionOperator.Equal, Guid.Parse(caseid));
                            convQuery.Criteria = filterConversation;

                            EntityCollection _conversationCollection = service.RetrieveMultiple(convQuery);

                            if (_conversationCollection.Entities != null && _conversationCollection.Entities.Count > 0)
                            {
                                foreach (Entity itemData in _conversationCollection.Entities)
                                {
                                    _conversationView = new conversationView();


                                    _conversationView.comments = itemData.Contains("tra_comment") ? (string)itemData.Attributes["tra_comment"] : string.Empty;
                                    _conversationView.commentBy = itemData.Contains("tra_commentby") ? (string)itemData.FormattedValues["tra_commentby"] : string.Empty;
                                    _conversationView.createdOn = (DateTime)itemData.Attributes["createdon"];
                                    _conversationView.conversationId = (Guid)itemData.Attributes["tra_conversationid"];
                                    _conversationView.isPortal = itemData.Contains("tra_showonlyattachmentsonportal") ? (Boolean)itemData.Attributes["tra_showonlyattachmentsonportal"] : false;

                                    _listConversation.Add(_conversationView);


                                }

                                if (_listConversation != null && _listConversation.Count > 0)
                                {
                                    foreach (var conversationItems in _listConversation)
                                    {

                                        if (conversationItems.commentBy == "Consumer" && conversationItems.isPortal == true)
                                        {
                                            QueryExpression annotQuery = new QueryExpression("annotation");
                                            annotQuery.ColumnSet = new ColumnSet(new string[]
                                            {
                                                    "filename"
                                            });
                                            FilterExpression _annotFilters = new FilterExpression(LogicalOperator.And);
                                            _annotFilters.AddCondition("objectid", ConditionOperator.Equal, conversationItems.conversationId);
                                            annotQuery.Criteria = _annotFilters;
                                            EntityCollection _annotationCollections = service.RetrieveMultiple(annotQuery);

                                            if (_annotationCollections.Entities != null && _annotationCollections.Entities.Count > 0)
                                            {
                                                foreach (Entity annotItems in _annotationCollections.Entities)
                                                {
                                                    annotation = new annotationClass();
                                                    annotation.annotId = (Guid)annotItems.Attributes["annotationid"];
                                                    annotation.filename = annotItems.Contains("filename") ? annotItems.Attributes["filename"].ToString() : string.Empty;

                                                    annotation.commentBy = conversationItems.commentBy;
                                                    annotation.IsportalAttach = conversationItems.isPortal;
                                                    annotationColl.Add(annotation);
                                                }


                                            }

                                        }


                                    }

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            _caseInfo._caseData = _caseData;
            _caseInfo.listAnnotation = annotationColl;
            return await Task.Run(() => _caseInfo);

        }

        public async Task<Boolean> forGotPassword(string email)
        {
            Boolean isforgotPassword = false;
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    OrganizationService organization = new OrganizationService();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    IOrganizationService service = organization.GetCRMService(configData);
                    if (service != null)
                    {
                        Entity _contact = new Entity("contact");
                       
                        QueryExpression _forgotPasswordQuery = new QueryExpression("contact");
                        _forgotPasswordQuery.ColumnSet = new ColumnSet("emailaddress1", "tra_resetpassword");
                        FilterExpression _forgotFilters = new FilterExpression(LogicalOperator.And);
                        _forgotFilters.AddCondition("emailaddress1", ConditionOperator.Equal, email.Trim());
                        _forgotFilters.AddCondition("tra_facebooktoken", ConditionOperator.Null);
                        _forgotFilters.AddCondition("tra_googletoken", ConditionOperator.Null);
                        _forgotPasswordQuery.Criteria = _forgotFilters;
                        EntityCollection _forgotPassCollection = service.RetrieveMultiple(_forgotPasswordQuery);
                        if (_forgotPassCollection != null && _forgotPassCollection.Entities.Count > 0)
                        {
                            _contact.Attributes.Add("tra_resetpassword", true);
                            _contact.Id = _forgotPassCollection.Entities[0].Id;
                            service.Update(_contact);
                            isforgotPassword = true;

                        }
                        else
                        {
                            isforgotPassword = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => isforgotPassword);
        }

        public async Task<consumerSuccess> cancelConsent(string caseid)
        {
            consumerSuccess consumer = new consumerSuccess();
            try
            {
                if (!string.IsNullOrEmpty(caseid))
                {
                    OrganizationService organization = new OrganizationService();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    IOrganizationService service = organization.GetCRMService(configData);
                    if (service != null)
                    {
                        Guid Id = Guid.Parse(caseid);
                        EntityReference SRItemReference = new EntityReference()
                        {
                            LogicalName = "incident",
                            Id = Id
                        };

                        SetStateRequest stateRequest = new SetStateRequest();
                        stateRequest.EntityMoniker = SRItemReference;
                        stateRequest.State = new OptionSetValue(2); //Code - Cancelled.
                        stateRequest.Status = new OptionSetValue(167490021); // Reason - Cancelled. 
                       
                        SetStateResponse response = new SetStateResponse();
                        response = (SetStateResponse)service.Execute(stateRequest);

                        if (response != null)
                        {
                            consumer.errorCode = HttpStatusCode.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => consumer);
        }

        public static string GetMimeTypeByWindowsRegistry(string filenameorextension)
        {
            try
            {
                string mimeType = "application/unknown";
                string ext = (filenameorextension.Contains(".")) ? System.IO.Path.GetExtension(filenameorextension).ToLower() : "." + filenameorextension;
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
                return mimeType;
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
               throw ex;
            }
           
        }

        public int getOptionSetValue(string attributename,IOrganizationService _service,string attributes)
        {
            int optionCode = 0;
            try
            {
                RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest
                {
                    EntityLogicalName = "tra_survey",
                    LogicalName = attributes,
                    RetrieveAsIfPublished = true
                };
                RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)_service.Execute(retrieveAttributeRequest);
                if (retrieveAttributeResponse != null)
                {
                    PicklistAttributeMetadata optionsetAttributeMetadata = retrieveAttributeResponse.AttributeMetadata as PicklistAttributeMetadata;
                    if (optionsetAttributeMetadata != null)
                    {
                        foreach (var options in optionsetAttributeMetadata.OptionSet.Options)
                        {
                            if(options.Label.LocalizedLabels[0].Label== attributename)
                            {
                                optionCode = options.Value.Value;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return optionCode;
        }

        public async Task<caseInformation> getFixedProbs(string caseid)
        {
            caseInformation _caseInfo = new caseInformation();
            
            try
            {
                if (!String.IsNullOrEmpty(caseid))
                {
                    OrganizationService organization = new OrganizationService();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    IOrganizationService service = organization.GetCRMService(configData);
                    if (service != null) {
                        
                            Entity _caseEntity = service.Retrieve("incident", Guid.Parse(caseid), new ColumnSet(true));
                            if (_caseEntity != null&& ((EntityReference)_caseEntity.Attributes["tra_complainttype"]).Name== "Quality of Service")
                            {
                                caseDetails _caseData = new caseDetails();
                                _caseData.caseType= _caseEntity.Contains("casetypecode") ? (string)_caseEntity.FormattedValues["casetypecode"] : string.Empty;
                                _caseData.caseNo = _caseEntity.Contains("ticketnumber") ? (string)_caseEntity.Attributes["ticketnumber"] : string.Empty;
                                _caseData.serviceProvider = _caseEntity.Contains("tra_serviceprovider") ? (string)((EntityReference)_caseEntity.Attributes["tra_serviceprovider"]).Name : string.Empty;
                                _caseData.serviceProviderId = _caseEntity.Contains("tra_serviceprovider") ? (Guid)((EntityReference)_caseEntity.Attributes["tra_serviceprovider"]).Id : Guid.Empty;
                                _caseData.complaintId = _caseEntity.Contains("tra_complainttype") ? (Guid)((EntityReference)_caseEntity.Attributes["tra_complainttype"]).Id : Guid.Empty;
                                _caseData.complaintType = _caseEntity.Contains("tra_complainttype") ? (string)((EntityReference)_caseEntity.Attributes["tra_complainttype"]).Name : string.Empty;
                                _caseData.fullname = _caseEntity.Contains("customerid") ? (string)((EntityReference)_caseEntity.Attributes["customerid"]).Name : string.Empty;
                                _caseData.contactid = _caseEntity.Contains("customerid") ? (Guid)((EntityReference)_caseEntity.Attributes["customerid"]).Id : Guid.Empty;
                                _caseData.road= _caseEntity.Contains("tra_street") ? (string)_caseEntity.Attributes["tra_street"] : string.Empty;
                                _caseData.block  = _caseEntity.Contains("tra_block") ? (string)_caseEntity.Attributes["tra_block"] : string.Empty;
                                _caseData.flat = _caseEntity.Contains("tra_flat") ? (string)_caseEntity.Attributes["tra_flat"] : string.Empty;
                                _caseData.building  = _caseEntity.Contains("tra_buildingflat") ? (string)_caseEntity.Attributes["tra_buildingflat"] : string.Empty;

                               _caseData.label = _caseEntity.Contains("tra_arealookup") ? (string)((EntityReference)_caseEntity.Attributes["tra_arealookup"]).Name : string.Empty;
                               _caseData.value = _caseEntity.Contains("tra_arealookup") ? (Guid)((EntityReference)_caseEntity.Attributes["tra_arealookup"]).Id : Guid.Empty;
                            //_caseData.address = street + " " + tra_block +" "+ tra_flat +" "+ tra_buildingflat;
                               DateTime date = (DateTime)_caseEntity.Attributes["createdon"];
                                _caseData.createOn = date.ToString("dd/MM/yyy", CultureInfo.InvariantCulture);
                                _caseInfo.IsFixedProbs = true;

                                _caseInfo._caseData = _caseData;



                            //}
                            //else
                            //{
                            //    _caseInfo.IsFixedProbs = false;
                            //}
                        }
                        else
                        {
                            _caseInfo.IsFixedProbs = false;

                        }
                    }
                    
                    
                }

            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(()=>_caseInfo);
        }

        public async Task<consumerSuccess> postfixedProbs(fixedProbsVM _fixedprobs)
        {
            consumerSuccess _consumerSuccessInfo = new consumerSuccess();
            try
            {
                if (_fixedprobs != null)
                {

                    OrganizationService organization = new OrganizationService();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    IOrganizationService service = organization.GetCRMService(configData);
                    if (service != null)
                    {
                        Entity _fixedprobsEntity = new Entity("tra_fixedprobs");
                        _fixedprobsEntity["tra_name"] = _fixedprobs.ticketno + "" + "- Fixed Probs";
                        _fixedprobsEntity["tra_casereference"] = new EntityReference("incident", _fixedprobs.caseId);
                        _fixedprobsEntity["tra_consumername"] = new EntityReference("contact", _fixedprobs.contactid);
                        _fixedprobsEntity["tra_operatorname"] = new EntityReference("tra_serviceprovider", _fixedprobs.serviceProviderId);
                        _fixedprobsEntity["tra_packagename"] = _fixedprobs.packageName != string.Empty ? _fixedprobs.packageName : string.Empty;
                        _fixedprobsEntity["tra_accessnetworktechnology"] = _fixedprobs.accessNetworkTech != string.Empty ? _fixedprobs.accessNetworkTech : string.Empty;
                        _fixedprobsEntity["tra_downloadthreshold"] = _fixedprobs.downloadThreshold != string.Empty ? _fixedprobs.downloadThreshold : string.Empty;
                        _fixedprobsEntity["tra_downloadspeed"] = _fixedprobs.downloadspeed != string.Empty ? _fixedprobs.downloadspeed : string.Empty;
                        _fixedprobsEntity["tra_uploadspeed"] = _fixedprobs.uploadSpeed != string.Empty ? _fixedprobs.uploadSpeed : string.Empty;
                        _fixedprobsEntity["tra_throttlespeeddownload"] = _fixedprobs.throttlespdDwnload != string.Empty ? _fixedprobs.throttlespdDwnload : string.Empty;
                        _fixedprobsEntity["tra_throttlespeedupload"] = _fixedprobs.throttlespdupload != string.Empty ? _fixedprobs.throttlespdupload : string.Empty;

                        _fixedprobsEntity["tra_flat"] = _fixedprobs.flat != string.Empty ? _fixedprobs.flat : string.Empty;
                        _fixedprobsEntity["tra_buildingvilla"] = _fixedprobs.buildingno != string.Empty ? _fixedprobs.buildingno : string.Empty;
                        _fixedprobsEntity["tra_block"] = _fixedprobs.blockno != string.Empty ? _fixedprobs.blockno : string.Empty;
                        if (_fixedprobs.area != Guid.Empty)
                        {
                            _fixedprobsEntity["tra_area"] = new EntityReference("tra_area", _fixedprobs.area);
                        }
                      
                        _fixedprobsEntity["tra_road"] = _fixedprobs.roadno != string.Empty ? _fixedprobs.roadno : string.Empty;
                        Guid id= service.Create(_fixedprobsEntity);
                        if (id != Guid.Empty)
                        {
                            Entity _entity = new Entity("annotation");
                            _entity["documentbody"]= _fixedprobs.signature.Substring(_fixedprobs.signature.IndexOf(",") + 1);
                            _entity["filename"] = _fixedprobs.fullname != string.Empty? _fixedprobs.fullname + " " + "Signature.jpg" : string.Empty;
                            _entity["objectid"] = new EntityReference("tra_fixedprobs", id);
                            Guid AnnotId = service.Create(_entity);

                            if (AnnotId != Guid.Empty)
                            {

                                Entity _incident = new Entity("incident");
                                _incident.Id = _fixedprobs.caseId;
                                _incident["statuscode"] = new OptionSetValue(167490029);
                                service.Update(_incident);
                                _consumerSuccessInfo.errorCode = HttpStatusCode.OK;
                                _consumerSuccessInfo.message = "Fixed Probs Created Successfully.";

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => _consumerSuccessInfo);
            
        }
        public async Task<List<GetNationality>> getArea()
        {
            List<GetNationality> listArea = new List<GetNationality>();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {
                    QueryExpression _queryEnquiry = new QueryExpression("tra_area");
                    _queryEnquiry.ColumnSet = new ColumnSet("tra_name");
                    FilterExpression _filterEnquiry = new FilterExpression(LogicalOperator.And);
                    _filterEnquiry.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryEnquiry.Criteria = _filterEnquiry;
                    EntityCollection _entityEnquiry = service.RetrieveMultiple(_queryEnquiry);
                    if (_entityEnquiry != null && _entityEnquiry.Entities != null && _entityEnquiry.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityEnquiry.Entities)
                        {

                            GetNationality _area = new GetNationality();
                            _area.label = item.Contains("tra_name") ? (string)item.Attributes["tra_name"] : string.Empty;
                            _area.value = item.Contains("tra_areaid") ? (Guid)item.Attributes["tra_areaid"] : Guid.Empty;
                            listArea.Add(_area);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;

            }
            return await Task.Run(() => listArea.OrderBy(a => a.label).Distinct().ToList());
        }
    }
   
}