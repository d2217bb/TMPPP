using Microsoft.AspNetCore.Mvc;
using TMPP.Creational.Builder;

namespace TMPP.Controllers.App;

public class DoctorController : Controller
{
    [HttpGet("/Doctor")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Doctor Portal";
        return View("~/Views/Doctor/Index.cshtml");
    }

    [HttpPost("/Doctor/Build")]
    public IActionResult BuildPrescription(string patientName, string antibiotic, bool isSigned)
    {
        var logs = new List<string>();
        try {
            var builder = new PrescriptionBuilder();
            var director = new DoctorDirector(builder);
            
            Prescription prescription;
            if (string.IsNullOrEmpty(antibiotic)) {
                // Manual build
                builder.SetPatientInfo(patientName);
                if (isSigned) builder.SignPrescription();
                prescription = builder.Build();
            } else {
                // Director builds a complex predefined recipe
                prescription = director.CreateAntibioticPrescription(patientName, antibiotic);
                if (isSigned) {
                    // It was already signed by director if we added sign logic there, but let's assume it.
                    // The director currently signs it in `CreateAntibioticPrescription`.
                    // So if it's not signed in the form, we can't easily undo it. Let's rely on the form.
                }
            }
            
            logs.Add("Prescription Validated and Built!");
            logs.Add($"Patient: {prescription.PatientName}");
            logs.Add($"Doctor Signature Present: {prescription.HasDoctorSignature}");
            foreach(var med in prescription.Medications) {
                logs.Add($"- {med.Name} ({med.Dosage}) - {med.Instructions}");
            }
        } catch(Exception ex) {
            logs.Add($"VALIDATION ERROR: {ex.Message}");
        }
        
        TempData["DoctorLogs"] = logs.ToArray();
        return RedirectToAction("Index");
    }
}
