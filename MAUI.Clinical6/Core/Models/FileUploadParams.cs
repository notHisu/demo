namespace Xamarin.Forms.Clinical6.Core.Models
{
    public class FileUploadParams
    {
        public IHavePermanentLink FlowProcess { get; }

        public int FieldId { get; }
        public int PatientId { get; }

        //int fieldId, int patientId
        public FileUploadParams(IHavePermanentLink havePermanentLink, int fieldId, int patientId)
        {
            FlowProcess = havePermanentLink;
            FieldId = fieldId;
            PatientId = patientId;
        }
    }
}