using Microsoft.Xrm.Sdk;
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
    public class ArabicConsumer : IArabicConsumer
    {
        List<string> mappings = new List<string>{
            { "image/bmp" },  // newly added format for zip file 
            { "application/msword"}, // newly added format for zip file 
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }, // newly added format for zip file 
            {"application/pdf"},        // newly added format for pdf file   
            {"image/gif"},
            {"application/octet-stream" },
            {"image/jpeg"},
            {"image/jpg" },
            {"image/png"},
            {"application/vnd.ms-powerpoint"},
            {"application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {"application/vnd.ms-xpsdocument"},
            {"image/pjpeg"},
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
                            _title.label = item.Contains("tra_titlear") ? (string)item.Attributes["tra_titlear"] : string.Empty;
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
                    _queryData.ColumnSet = new ColumnSet("tra_nameofserviceproviderar");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityCase = service.RetrieveMultiple(_queryData);
                    if (_entityCase != null && _entityCase.Entities != null && _entityCase.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityCase.Entities)
                        {

                            getServiceProviders _getsrvcPrdInfo = new getServiceProviders();
                            _getsrvcPrdInfo.label = item.Contains("tra_nameofserviceproviderar") ? (string)item.Attributes["tra_nameofserviceproviderar"] : string.Empty;
                            _getsrvcPrdInfo.value = item.Contains("tra_serviceproviderid") ? (Guid)item.Attributes["tra_serviceproviderid"] : Guid.Empty;
                            lystservcprdList.Add(_getsrvcPrdInfo);
                        }


                    }


                }


            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => lystservcprdList);
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
                                              "tra_servicear"
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
                            _srvcData.label = data.Attributes.Contains("tra_servicear") ? (string)data.Attributes["tra_servicear"].ToString() : string.Empty;
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

            return await Task.Run(() => servicesList);
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
                    _queryData.ColumnSet = new ColumnSet("tra_complainttypear");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityCase = service.RetrieveMultiple(_queryData);
                    if (_entityCase != null && _entityCase.Entities != null && _entityCase.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityCase.Entities)
                        {
                            getComplaint _getcomlaint = new getComplaint();
                            _getcomlaint.label = item.Contains("tra_complainttypear") ? (string)item.Attributes["tra_complainttypear"] : string.Empty;
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
            return await Task.Run(() => lystComplaints);
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
                    _queryExp.ColumnSet = new ColumnSet("tra_complaintsubtypear");

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
                            complaintSub.label = data.Attributes.Contains("tra_complaintsubtypear") ? (string)data.Attributes["tra_complaintsubtypear"].ToString() : string.Empty;
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
            return await Task.Run(() => complaintSubList);

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
                                             <attribute name='tra_documentar' />
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
                            document.document = item.Contains("tra_documentar") ? (string)item.Attributes["tra_documentar"] : string.Empty;
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

            return await Task.Run(() => listDocuments);
        }

        public async Task<caseDetail> AddcomplaintsAr()
        {

            caseDetail complaintInfo = new caseDetail();
            documentsData documentCom = null;
            List<documentsData> listdocuments = new List<documentsData>();
            ComplaintObject _complaintObj = new ComplaintObject();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);


                if (service != null)
                {
                    var httpRequestData = HttpContext.Current.Request;



                    if (httpRequestData != null && httpRequestData.Form.Count > 0)
                    {

                        //////////////////MIME Type Checking for Uploaded Files/////////////
                        //foreach (string filesItem in httpRequestData.Files)
                        //{
                        //    documentCom = new documentsData();
                        //     HttpPostedFile httpPostedFiles = httpRequestData.Files[filesItem];
                        //    Stream fs = httpPostedFiles.InputStream;
                        //    BinaryReader br = new System.IO.BinaryReader(fs);
                        //    Byte[] fileBytes = br.ReadBytes((Int32)fs.Length);
                        //    documentCom.documents= Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                        //    listdocuments.Add(documentCom);
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

                        DateTime date = datecreate(_complaintObj.complaintDate);
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

                            if (_complaintObj.complaintSubType != "undefined")
                            {
                                _complaintEntity["tra_complaintsubtype"] = new EntityReference("tra_complaintsubtype", Guid.Parse(_complaintObj.complaintSubType));
                            }

                            _complaintEntity["tra_disputednumber"] = _complaintObj.disputeNo.Trim();
                            _complaintEntity["tra_serviceprovidercasereference"] = _complaintObj.caseRefNo.Trim();

                            _complaintEntity["caseorigincode"] = new OptionSetValue(3);

                            if (_complaintObj.subscriptionType == "اجل الدفع")
                            {
                                _complaintEntity["tra_subscriptiontype"] = new OptionSetValue(Convert.ToInt32(167490001));
                            }
                            else if (_complaintObj.subscriptionType == "مسبق الدفع")
                            {
                                _complaintEntity["tra_subscriptiontype"] = new OptionSetValue(Convert.ToInt32(167490000));
                            }
                            _complaintEntity["tra_dateofcomplaintagainstserviceprovider"] = Convert.ToDateTime(date);
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


                            if (_complaintObj.complaintName == "الفواتير")
                            {
                                string disputeAmount = _complaintObj.disputedAmount.Replace(".د.ب", "");
                                decimal amount = Convert.ToDecimal(disputeAmount);

                                _complaintEntity["tra_disputedamount"] = new Money(amount);
                            }

                            else if (_complaintObj.complaintName == "جودة الخدمة")
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
                                Entity _complaintEnt = service.Retrieve("incident", complaintId, new ColumnSet("ticketnumber"));
                                if (_complaintEnt != null)
                                {
                                    caseNum = _complaintEnt.Attributes["ticketnumber"].ToString();
                                }

                                if (caseNum != string.Empty && httpRequestData.Files.Count > 0)
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
                                        //if (listdocuments != null && listdocuments.Count > 0)
                                        //{
                                        //   for(int w = 0; w < listdocuments.Count; w++)
                                        //    {
                                        //        lystdocumentBody[w].documentbody = listdocuments[w].documents;
                                        //    }
                                        //}

                                        if (lystdocumentBody != null && lystdocumentBody.Count > 0)
                                        {
                                            for (int i = 0; i < lystdocumentBody.Count; i++)
                                            {
                                                if (lystdocumentBody[i].documentType != "المستندات الداعمة (إن وجدت)")
                                                {

                                                    Entity conversation = new Entity("tra_conversation");
                                                    conversation.Attributes.Add("tra_comment", lystdocumentBody[i].documentType);
                                                    conversation["tra_showonlyattachmentsonportal"] = Convert.ToBoolean(true);

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

                                            var filesData = lystdocumentBody.Where(a => a.documentType == "المستندات الداعمة (إن وجدت)").ToList();
                                            if (filesData != null && filesData.Count > 0)
                                            {
                                                Entity _conversionEntity = new Entity("tra_conversation");

                                                _conversionEntity["tra_caseid"] = new EntityReference("incident", complaintId);
                                                _conversionEntity["tra_comment"] = "المستندات الداعمة (إن وجدت) ";
                                                _conversionEntity.Attributes.Add("tra_commentby", new OptionSetValue(1));
                                                _conversionEntity["tra_showonlyattachmentsonportal"] = Convert.ToBoolean(true);
                                                _conversionEntity["tra_consumer"] = Convert.ToBoolean(true);
                                                Guid conversationIdforMultiples = service.Create(_conversionEntity);

                                                if (conversationIdforMultiples != Guid.Empty)
                                                {
                                                    for (int ij = 0; ij < filesData.Count; ij++)
                                                    {
                                                        if (filesData[ij].documentType == "المستندات الداعمة (إن وجدت)" && filesData[ij].documentType.Contains("المستندات الداعمة (إن وجدت)"))
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
                                                complaintInfo.message = "Complaint created successfully";
                                            }
                                        }
                                    }


                                }
                                else
                                {
                                    complaintInfo.caseNo = caseNum;
                                    complaintInfo.errorCode = HttpStatusCode.OK;
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
                    _queryEnquiry.ColumnSet = new ColumnSet("tra_enquirytypear");
                    FilterExpression _filterEnquiry = new FilterExpression(LogicalOperator.And);
                    _filterEnquiry.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryEnquiry.Criteria = _filterEnquiry;
                    EntityCollection _entityEnquiry = service.RetrieveMultiple(_queryEnquiry);
                    if (_entityEnquiry != null && _entityEnquiry.Entities != null && _entityEnquiry.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityEnquiry.Entities)
                        {

                            enquiryType _enquiry = new enquiryType();
                            _enquiry.label = item.Contains("tra_enquirytypear") ? (string)item.Attributes["tra_enquirytypear"] : string.Empty;
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
            return await Task.Run(() => listEnquiry);
        }
        public async Task<caseDetail> createEnquiry()
        {
            caseDetail _enquiryData = new caseDetail();
            //  documentsData documentsEnq = null;
            List<documentsData> listdocEnq = new List<documentsData>();
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
                        //    documentsEnq = new documentsData();
                        //    HttpPostedFile httpPostedFiles = httpRequest.Files[filesItem];
                        //    Stream fs = httpPostedFiles.InputStream;
                        //    BinaryReader br = new System.IO.BinaryReader(fs);
                        //    Byte[] fileBytes = br.ReadBytes((Int32)fs.Length);
                        //    documentsEnq.documents= Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                        //    listdocEnq.Add(documentsEnq);
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
                    if (httpRequest != null && httpRequest.Form.Count > 0)
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

                                Entity enquiryEnt = service.Retrieve("incident", enquiryId, new ColumnSet("ticketnumber"));

                                if (enquiryEnt != null)
                                {

                                    caseNumber = enquiryEnt.Attributes["ticketnumber"].ToString();
                                }


                            }
                            /////////////////////create conversation ////////////////////////////////////
                            if (httpRequest.Files.Count > 0)
                            {


                                Entity _conversionEntity = new Entity("tra_conversation");

                                _conversionEntity["tra_caseid"] = new EntityReference("incident", enquiryId);
                                _conversionEntity.Attributes.Add("tra_commentby", new OptionSetValue(1));
                                _conversionEntity["tra_comment"] = "المستندات الداعمة (إن وجدت)";
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

                                        //if (listdocEnq != null && listdocEnq.Count > 0)
                                        //{
                                        //    for(int uy = 0; uy < listdocEnq.Count; uy++)
                                        //    {
                                        //        lystdocumentBody[uy].documentbody = listdocEnq[uy].documents;
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

            return await Task.Run(() => _enquiryData);

        }

        public async Task<caseDetail> createSuggestion()
        {
            caseDetail suggestionInfos = new caseDetail();
            // documentsData _docSugg = null;
            List<documentsData> listdocSugg = new List<documentsData>();
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
                        //    _docSugg = new documentsData();
                        //    HttpPostedFile httpPostedFiles = httpRequest.Files[filesItem];
                        //    Stream fs = httpPostedFiles.InputStream;
                        //    BinaryReader br = new System.IO.BinaryReader(fs);
                        //    Byte[] fileBytes = br.ReadBytes((Int32)fs.Length);
                        //    _docSugg.documents = Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                        //    listdocSugg.Add(_docSugg);
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
                    if (httpRequest != null && httpRequest.Form.Count > 0)
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

                                Entity _suggestionEntity = service.Retrieve("incident", suggestionId, new ColumnSet("ticketnumber"));
                                if (_suggestionEntity != null)
                                {
                                    caseNumber = _suggestionEntity.Attributes["ticketnumber"].ToString();
                                }


                            }
                            /////////////////////create conversation ////////////////////////////////////
                            if (httpRequest.Files.Count > 0)
                            {
                                Entity _conversionEntity = new Entity("tra_conversation");

                                _conversionEntity["tra_caseid"] = new EntityReference("incident", suggestionId);
                                _conversionEntity.Attributes.Add("tra_commentby", new OptionSetValue(1));
                                _conversionEntity["tra_comment"] = "المستندات الداعمة (إن وجدت)";
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

                                        //if (listdocSugg != null && listdocSugg.Count > 0)
                                        //{

                                        //   for(int qo = 0; qo < listdocSugg.Count; qo++)
                                        //   {
                                        //        lystdocumentBody[qo].documentbody = listdocSugg[qo].documents;
                                        //   }
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
                                "ticketnumber","createdon","casetypecode","customerid","tra_casestatusforconsumer"

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
                                    cases.createdDate = (DateTime)item.Attributes["createdon"];
                                    DateTime dt = (DateTime)item.Attributes["createdon"];
                                    cases.date_submitted = dt.ToString("dd/MM/yyy", CultureInfo.InvariantCulture);

                                    string caseType = item.Attributes.Contains("casetypecode") ? (string)item.FormattedValues["casetypecode"] : string.Empty;
                                    cases.case_type = getCaseTypeAr(caseType);


                                    ///////////////////

                                    string status = item.Attributes.Contains("tra_casestatusforconsumer") ? (string)item.FormattedValues["tra_casestatusforconsumer"].ToString() : string.Empty;

                                    cases.case_status = getArabicStatusType(status);


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
                                _caseinfo.stateCode = item.Contains("statecode") ? (string)item.FormattedValues["statecode"] : string.Empty;

                                string caseTypes = item.Contains("casetypecode") ? (string)item.FormattedValues["casetypecode"] : string.Empty;

                                if (caseTypes == "Complaint")
                                {
                                    _caseinfo.caseDescription = item.Contains("tra_casequestion1") ? (string)item.Attributes["tra_casequestion1"] : string.Empty;
                                }
                                else if (caseTypes == "Enquiry" || caseTypes == "Suggestion")
                                {
                                    _caseinfo.caseDescription = item.Contains("description") ? (string)item.Attributes["description"] : string.Empty;
                                }
                                _caseinfo.caseType = getCaseTypeAr(caseTypes);
                                string statusCase = item.Contains("tra_casestatusforconsumer") ? (string)item.FormattedValues["tra_casestatusforconsumer"] : string.Empty;
                                _caseinfo.status = getArabicStatusType(statusCase);

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
                                        _conversationView.isPortal = itemData.Contains("tra_showonlyattachmentsonportal") ? (Boolean)itemData.Attributes["tra_showonlyattachmentsonportal"] : false;
                                        _conversationView.IsConsumer = itemData.Contains("tra_consumer") ? (Boolean)itemData.Attributes["tra_consumer"] : false;
                                        _conversationView.conversationId = (Guid)itemData.Attributes["tra_conversationid"];

                                        _listConversation.Add(_conversationView);

                                    }


                                    if (_listConversation != null && _listConversation.Count > 0)
                                    {

                                        foreach (var conversationItems in _listConversation)
                                        {

                                            if (((conversationItems.commentBy == "Consumer" && conversationItems.isPortal == true) || (conversationItems.IsConsumer == true)))
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
            return await Task.Run(() => caseInfos);
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
                        annotation.documentbody = _annotationCollections.Entities[0].Attributes["documentbody"].ToString();
                        annotation.filename = _annotationCollections.Entities[0].Attributes["filename"].ToString();
                        annotation.mimeType = EnglishConsumer.GetMimeTypeByWindowsRegistry(annotation.filename);

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

        public async Task<List<GetNationality>> getNationsAr()
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
                    _queryData.ColumnSet = new ColumnSet("tra_nationalityar");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityNation = service.RetrieveMultiple(_queryData);
                    if (_entityNation != null && _entityNation.Entities != null && _entityNation.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityNation.Entities)
                        {
                            _getNation = new GetNationality();
                            _getNation.label = item.Contains("tra_nationalityar") ? (string)item.Attributes["tra_nationalityar"] : string.Empty;
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
            return await Task.Run(() => lystNations);
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
                        _queryContactInfo.ColumnSet = new ColumnSet("emailaddress1", "fullname", "firstname", "lastname", "tra_idtype", "tra_idnumber", "tra_nationality", "mobilephone", "birthdate", "tra_countrycode");
                        FilterExpression _filterContactData = new FilterExpression(LogicalOperator.And);
                        _filterContactData.AddCondition("statecode", ConditionOperator.Equal, 0);
                        _filterContactData.AddCondition("contactid", ConditionOperator.Equal, Guid.Parse(contactid));
                        _queryContactInfo.Criteria = _filterContactData;
                        EntityCollection _contactCollection = service.RetrieveMultiple(_queryContactInfo);

                        if (_contactCollection != null && _contactCollection.Entities.Count > 0)
                        {
                            _getContactInfo.lastname = _contactCollection.Entities[0].Contains("lastname") ? (string)_contactCollection.Entities[0].Attributes["lastname"] : string.Empty;
                            _getContactInfo.firstname = _contactCollection.Entities[0].Contains("firstname") ? (string)_contactCollection.Entities[0].Attributes["firstname"] : string.Empty;
                            _getContactInfo.nationality = _contactCollection.Entities[0].Contains("tra_nationality") ? (string)((EntityReference)_contactCollection.Entities[0].Attributes["tra_nationality"]).Name : null;
                            _getContactInfo.nationalityId = _contactCollection.Entities[0].Contains("tra_nationality") ? (Guid)((EntityReference)_contactCollection.Entities[0].Attributes["tra_nationality"]).Id : Guid.Empty;
                            _getContactInfo.idnumber = _contactCollection.Entities[0].Contains("tra_idnumber") ? _contactCollection.Entities[0].Attributes["tra_idnumber"].ToString() : string.Empty;
                            _getContactInfo.createdon = _contactCollection.Entities[0].Contains("birthdate") ? _contactCollection.Entities[0].Attributes["birthdate"].ToString() : string.Empty;
                            _getContactInfo.countrycode = _contactCollection.Entities[0].Contains("tra_countrycode") ? _contactCollection.Entities[0].Attributes["tra_countrycode"].ToString() : string.Empty;
                            string IdType = _contactCollection.Entities[0].Contains("tra_idtype") ? _contactCollection.Entities[0].FormattedValues["tra_idtype"].ToString() : string.Empty;
                            if (IdType == "CPR Number")
                            {
                                _getContactInfo.id_type = "البطاقة الشخصية";
                            }
                            else if (IdType == "Passport Number")
                            {
                                _getContactInfo.id_type = "الجواز";
                            }
                            _getContactInfo.emailaddress = _contactCollection.Entities[0].Contains("emailaddress1") ? _contactCollection.Entities[0].Attributes["emailaddress1"].ToString() : string.Empty;
                            _getContactInfo.contactid = (Guid)_contactCollection.Entities[0].Attributes["contactid"];
                            _getContactInfo.contactno = _contactCollection.Entities[0].Contains("mobilephone") ? _contactCollection.Entities[0].Attributes["mobilephone"].ToString() : string.Empty;






                        }


                    }

                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => _getContactInfo);
        }

        public async Task<consumerSuccess> updateUserAr(TraUser _updateuser)
        {
            consumerSuccess consumersUpdate = new consumerSuccess();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);

                if (service != null && _updateuser != null)
                {
                    Entity _contactUpdates = new Entity("contact", _updateuser.ContactId);
                    if (_updateuser != null)
                    {

                        _contactUpdates["firstname"] = _updateuser.firstname.Trim();
                        _contactUpdates["lastname"] = _updateuser.lastname.Trim();
                        _contactUpdates["tra_nationality"] = new EntityReference("tra_nationality", _updateuser.nationId);
                        _contactUpdates["tra_countrycode"] = _updateuser.countryCode.Trim();

                        _contactUpdates["tra_idnumber"] = _updateuser.CprPassportNo;

                        DateTime dateofBirths = datecreate(_updateuser.DOB);
                        _contactUpdates["birthdate"] = dateofBirths;
                        _contactUpdates["emailaddress1"] = _updateuser.emailId;
                        _contactUpdates["mobilephone"] = _updateuser.contact.Trim();

                        if (_updateuser.IdType == "البطاقة الشخصية")
                        {
                            _contactUpdates["tra_idtype"] = new OptionSetValue(1);
                        }
                        else if (_updateuser.IdType == "الجواز")
                        {
                            _contactUpdates["tra_idtype"] = new OptionSetValue(2);
                        }

                    }

                    service.Update(_contactUpdates);
                    consumersUpdate.fullname = _updateuser.firstname + " " + _updateuser.lastname;
                    consumersUpdate.firstname = _updateuser.firstname;
                    consumersUpdate.contactId = _updateuser.ContactId;
                    consumersUpdate.errorCode = HttpStatusCode.OK;
                    consumersUpdate.message = "Contact successfully updated";

                }
            }
            catch (Exception ex)
            {

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => consumersUpdate);
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
            return await Task.Run(() => _changePasswordInfo);
        }
        public async Task<consumerSuccess> indivisualArUser(TraUser user)
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


                        if (user.IdType == "البطاقة الشخصية")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(1);

                        }

                        else if (user.IdType == "الجواز")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(2);

                        }

                        _contactEntity["tra_nationality"] = new EntityReference("tra_nationality", Guid.Parse(user.nationality));

                        DateTime dateofBirths1 = datecreate(user.DOB.Trim());
                        _contactEntity["birthdate"] = dateofBirths1;
                        _contactEntity["tra_consumertype"] = new OptionSetValue(1);
                        _contactEntity.Attributes.Add("tra_title", new EntityReference("tra_title", user.titleId));
                        _contactEntity["tra_countrycode"] = user.countryCode.Trim();
                        _contactEntity["mobilephone"] = user.contact.Trim();
                        _contactEntity["tra_source"] = new OptionSetValue(1);

                        if (user.socialProvider != null && user.socialProvider != string.Empty)
                        {

                            if (user.socialProvider.Trim() == "Google" || user.socialProvider.Trim() == "Facebook")
                            {
                                if (user.socialProvider.Trim() == "Google")
                                {
                                    _contactEntity["tra_googletoken"] = user.socialProviderKey.Trim();
                                }
                                else if (user.socialProvider == "Facebook")
                                {
                                    _contactEntity["tra_facebooktoken"] = user.socialProviderKey.Trim();
                                }
                            }
                        }

                        else
                        {
                            //Changes By shweta
                            string PasswordAr = Encryption.Encrypt(user.password.Trim());
                            _contactEntity.Attributes.Add("tra_password", PasswordAr);//Changes
                            _contactEntity.Attributes.Add("tra_iscreatedfromportal", true);
                        }




                        _contactEntity["tra_idnumber"] = user.CprPassportNo.Trim();

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
            return await Task.Run(() => consumers);
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



                        if (buisnessuser.IdType == "البطاقة الشخصية")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(1);

                        }
                        else if (buisnessuser.IdType == "الجواز")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(2);

                        }

                        _contactEntity["tra_nationality"] = new EntityReference("tra_nationality", Guid.Parse(buisnessuser.nationality.Trim()));

                        DateTime dateofBirths2 = datecreate(buisnessuser.DOB);
                        _contactEntity.Attributes.Add("tra_title", new EntityReference("tra_title", buisnessuser.titleId));
                        _contactEntity["birthdate"] = dateofBirths2;
                        _contactEntity["tra_consumertype"] = new OptionSetValue(2);
                        _contactEntity["tra_countrycode"] = buisnessuser.countryCode.Trim();
                        _contactEntity["mobilephone"] = buisnessuser.contact.Trim();
                        _contactEntity["tra_companyname"] = buisnessuser.companyName.Trim();
                        _contactEntity["tra_crnumber"] = buisnessuser.commercialNo.Trim();
                        _contactEntity["tra_source"] = new OptionSetValue(1);
                        _contactEntity["tra_idnumber"] = buisnessuser.CprPassportNo.Trim();

                        if (buisnessuser.socialProvider != null && buisnessuser.socialProvider != string.Empty)
                        {
                            if (buisnessuser.socialProvider.Trim() == "Google" || buisnessuser.socialProvider.Trim() == "Facebook")
                            {
                                if (buisnessuser.socialProvider.Trim() == "Google")
                                {
                                    _contactEntity["tra_googletoken"] = buisnessuser.socialProviderKey.Trim();
                                }
                                else if (buisnessuser.socialProvider == "Facebook")
                                {
                                    _contactEntity["tra_facebooktoken"] = buisnessuser.socialProviderKey.Trim();
                                }
                            }

                        }
                        else
                        {
                            //Changes By shweta
                            string BusinessUserARPass = Encryption.Encrypt(buisnessuser.password.Trim());
                            _contactEntity.Attributes.Add("tra_password", BusinessUserARPass);//Changes
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
            return await Task.Run(() => consumers);
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


                        if (governmentuser.IdType == "البطاقة الشخصية")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(1);

                        }
                        else if (governmentuser.IdType == "الجواز")
                        {
                            _contactEntity["tra_idtype"] = new OptionSetValue(2);

                        }
                        _contactEntity["tra_nationality"] = new EntityReference("tra_nationality", Guid.Parse(governmentuser.nationality.Trim()));

                        DateTime dateofBirths3 = datecreate(governmentuser.DOB.Trim());
                        _contactEntity["birthdate"] = dateofBirths3;
                        _contactEntity.Attributes.Add("tra_title", new EntityReference("tra_title", governmentuser.titleId));
                        _contactEntity["tra_consumertype"] = new OptionSetValue(3);
                        _contactEntity["tra_countrycode"] = governmentuser.countryCode.Trim();
                        _contactEntity["mobilephone"] = governmentuser.contact.Trim();
                        _contactEntity["tra_companyname"] = governmentuser.governmentEntity.Trim();
                        _contactEntity["tra_source"] = new OptionSetValue(1);

                        _contactEntity["tra_idnumber"] = governmentuser.CprPassportNo.Trim();

                        if (governmentuser.socialProvider != null && governmentuser.socialProvider != string.Empty)
                        {
                            if (governmentuser.socialProvider.Trim() == "Google" || governmentuser.socialProvider.Trim() == "Facebook")
                            {
                                if (governmentuser.socialProvider.Trim() == "Google")
                                {
                                    _contactEntity["tra_googletoken"] = governmentuser.socialProviderKey.Trim();
                                }
                                else if (governmentuser.socialProvider == "Facebook")
                                {
                                    _contactEntity["tra_facebooktoken"] = governmentuser.socialProviderKey.Trim();
                                }
                            }
                        }
                        else
                        {
                            //Changes By shweta
                            string GovernmentUserPass = Encryption.Encrypt(governmentuser.password.Trim());
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
            return await Task.Run(() => consumers);
        }

        public async Task<caseDetail> userCommentAr()
        {
            caseDetail _consumerReply = new caseDetail();
            try
            {
                OrganizationService organization = new OrganizationService();
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                IOrganizationService service = organization.GetCRMService(configData);
                if (service != null)
                {
                    var httpContext = HttpContext.Current.Request.Form;

                    if (httpContext.Count > 0)
                    {
                        replyModel _replyModel = new replyModel();
                        _replyModel.caseId = Guid.Parse(httpContext["caseId"]);
                        _replyModel.caseNo = httpContext["CaseNo"];
                        _replyModel.replyComment = httpContext["replyComments"];

                        if (_replyModel != null)
                        {
                            Entity _conversionEntity = new Entity("tra_conversation");

                            _conversionEntity["tra_caseid"] = new EntityReference("incident", _replyModel.caseId);
                            _conversionEntity["tra_comment"] = _replyModel.replyComment;
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

                                var httpFiles = HttpContext.Current.Request.Files;

                                if (httpFiles.Count > 0)
                                {
                                    foreach (string file in httpFiles)
                                    {
                                        postedFiles = new postedFile();
                                        postedFiles.httppostedFile = httpFiles[file];
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

                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return await Task.Run(() => _consumerReply);
        }


        public async Task<caseInformation> getConsumerConsentInfo(string caseid)

        {
            caseInformation _caseInfo = new caseInformation();
            caseDetails _caseData = null;
            conversationView _conversationView = null;
            annotationClass annotation = null;
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

                        Entity ConsentCancel = service.Retrieve("incident", Guid.Parse(caseid.Trim()), new ColumnSet(true));

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
                                EntityReference entityRef = ((EntityReference)_entity.Entities[0].Attributes["tra_serviceprovider"]);
                                if (entityRef != null && entityRef.LogicalName != null && entityRef.Id != null)
                                {
                                    Entity entity = service.Retrieve(entityRef.LogicalName, entityRef.Id, new ColumnSet("tra_nameofserviceproviderar"));
                                    _caseData.serviceProvider = entity.Contains("tra_nameofserviceproviderar") ? entity.Attributes["tra_nameofserviceproviderar"].ToString() : string.Empty;

                                }
                                EntityReference entityRef1 = ((EntityReference)_entity.Entities[0].Attributes["tra_complainttype"]);
                                if (entityRef1 != null && entityRef1.LogicalName != null && entityRef1.Id != null)
                                {
                                    Entity entity1 = service.Retrieve(entityRef1.LogicalName, entityRef1.Id, new ColumnSet("tra_complainttypear"));
                                    _caseData.complaintType = entity1.Contains("tra_complainttypear") ? entity1.Attributes["tra_complainttypear"].ToString() : string.Empty;

                                }
                                EntityReference entityRef2 = ((EntityReference)_entity.Entities[0].Attributes["tra_service"]);
                                if (entityRef2 != null && entityRef2.LogicalName != null && entityRef2.Id != null)
                                {
                                    Entity entity2 = service.Retrieve(entityRef2.LogicalName, entityRef2.Id, new ColumnSet("tra_servicear"));
                                    _caseData.service = entity2.Contains("tra_servicear") ? entity2.Attributes["tra_servicear"].ToString() : string.Empty;

                                }

                                _caseData.caseNo = _entity.Entities[0].Contains("ticketnumber") ? (string)_entity.Entities[0].Attributes["ticketnumber"] : string.Empty;

                                _caseData.complaintId = _entity.Entities[0].Contains("tra_complainttype") ? (Guid)((EntityReference)_entity.Entities[0].Attributes["tra_complainttype"]).Id : Guid.Empty;

                                _caseData.serviceId = _entity.Entities[0].Contains("tra_service") ? (Guid)((EntityReference)_entity.Entities[0].Attributes["tra_service"]).Id : Guid.Empty;

                                _caseData.serviceProviderId = _entity.Entities[0].Contains("tra_serviceprovider") ? (Guid)((EntityReference)_entity.Entities[0].Attributes["tra_serviceprovider"]).Id : Guid.Empty;
                                _caseData.fullname = _entity.Entities[0].Contains("customerid") ? (string)((EntityReference)_entity.Entities[0].Attributes["customerid"]).Name : null;
                                _caseData.contactid = _entity.Entities[0].Contains("customerid") ? (Guid)((EntityReference)_entity.Entities[0].Attributes["customerid"]).Id : Guid.Empty;
                                DateTime date = (DateTime)_entity.Entities[0].Attributes["createdon"];
                                _caseData.submitComplaintDate = (DateTime)_entity.Entities[0].Attributes["createdon"];
                                _caseData.createOn = date.ToString("dd/MM/yyy", CultureInfo.InvariantCulture);

                                string caseTypes = _entity.Entities[0].Contains("casetypecode") ? (string)_entity.Entities[0].FormattedValues["casetypecode"] : string.Empty;
                                if (caseTypes == "Complaint")
                                {
                                    _caseData.caseDescription = _entity.Entities[0].Contains("tra_casequestion1") ? (string)_entity.Entities[0].Attributes["tra_casequestion1"] : string.Empty;
                                }
                                else if (caseTypes == "Enquiry" || caseTypes == "Suggestion")
                                {
                                    _caseData.caseDescription = _entity.Entities[0].Contains("description") ? (string)_entity.Entities[0].Attributes["description"] : string.Empty;
                                }

                                _caseData.caseType = getCaseTypeAr(caseTypes);
                                _caseData.caseRefNo = _entity.Entities[0].Contains("tra_serviceprovidercasereference") ? (string)_entity.Entities[0].Attributes["tra_serviceprovidercasereference"] : string.Empty;
                                if (_caseData.caseType == "شكوى")
                                {
                                    if (_entity.Entities[0].Contains("tra_dateofcomplaintagainstserviceprovider"))
                                    {
                                        DateTime? dt = _entity.Entities[0].Contains("tra_dateofcomplaintagainstserviceprovider") ? (DateTime)_entity.Entities[0].Attributes["tra_dateofcomplaintagainstserviceprovider"] : (DateTime?)null;
                                        _caseData.complaintAgainstSp = dt;

                                    }

                                }
                                _caseData.disputNo = _entity.Entities[0].Contains("tra_disputednumber") ? (string)_entity.Entities[0].Attributes["tra_disputednumber"] : string.Empty;
                                _caseData.caseDescription = _entity.Entities[0].Contains("tra_casequestion1") ? (string)_entity.Entities[0].Attributes["tra_casequestion1"] : string.Empty;
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
                                        _conversationView.isPortal = itemData.Contains("tra_showonlyattachmentsonportal") ? (Boolean)itemData.Attributes["tra_showonlyattachmentsonportal"] : true;

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


                    if (_consentdata.caseType == "شكوى")
                    {
                        _consentEntity["casetypecode"] = new OptionSetValue(Convert.ToInt32(1));
                    }
                    else if (_consentdata.caseType == "استفسار")
                    {
                        _consentEntity["casetypecode"] = new OptionSetValue(Convert.ToInt32(2));
                    }
                    else if (_consentdata.caseType == "اقتراح")
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


                    if (_consentdata.caseType == "شكوى" && _consentdata.complaintDate != "")
                    {
                        DateTime complaintDate = datecreate(_consentdata.complaintDate);

                        _consentEntity["tra_dateofcomplaintagainstserviceprovider"] = complaintDate;
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
                    if (!string.IsNullOrEmpty(caseid))
                    {
                        QueryExpression _queryData = new QueryExpression("incident");
                        _queryData.ColumnSet = new ColumnSet(new string[]
                        {
                               "ticketnumber","createdon","casetypecode","statecode","tra_casequestion1","tra_casestatusforconsumer","description"
                              });


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
                            string caseTypes = _entity.Entities[0].Contains("casetypecode") ? (string)_entity.Entities[0].FormattedValues["casetypecode"] : string.Empty;
                            if (caseTypes == "Complaint")
                            {
                                _caseData.caseDescription = _entity.Entities[0].Contains("tra_casequestion1") ? (string)_entity.Entities[0].Attributes["tra_casequestion1"] : string.Empty;
                            }
                            else if (caseTypes == "Enquiry" || caseTypes == "Suggestion")
                            {
                                _caseData.caseDescription = _entity.Entities[0].Contains("description") ? (string)_entity.Entities[0].Attributes["description"] : string.Empty;
                            }

                            _caseData.caseType = getCaseTypeAr(caseTypes);

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
        /// <summary> 
        /// ////////////////////////////////////////////////////
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>

        public DateTime datecreate(string date)
        {

            try
            {
                var cultureInfo = CultureInfo.CreateSpecificCulture("ar-SA");
                cultureInfo.DateTimeFormat.Calendar = cultureInfo.OptionalCalendars[5];

                var dateTimeString = date;
                var dateTime = DateTime.Parse(dateTimeString, cultureInfo);
                return dateTime;
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
        }

        public string getArabicStatusType(string status)
        {
            string case_status = string.Empty;
            try
            {

                if (status == "Active")
                {
                    case_status = "نشيط";

                }
                else if (status == "Closed")
                {
                    case_status = "مغلق";

                }
                else if (status == "Rejected")
                {
                    case_status = "مرفوض";
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }

            return case_status;
        }

        public string getCaseTypeAr(string casetypes)
        {
            string case_type = string.Empty;
            try
            {

                if (casetypes == "Complaint")
                {
                    case_type = "شكوى";
                }
                else if (casetypes == "Enquiry")
                {
                    case_type = "استفسار";
                }
                else if (casetypes == "Suggestion")
                {
                    case_type = "اقتراح";
                }
            }
            catch (Exception ex)
            {
                Exceptions.exceptionHandler(ex.ToString());
                throw ex;
            }
            return case_type;
        }
    }
}