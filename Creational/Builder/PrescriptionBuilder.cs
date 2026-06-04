namespace TMPP.Creational.Builder;

public class Medication
{
    public string Name { get; set; } = "";
    public string Dosage { get; set; } = "";
    public string Instructions { get; set; } = "";
}

public class Prescription
{
    public string PatientName { get; set; } = "";
    public List<Medication> Medications { get; set; } = new();
    public bool HasDoctorSignature { get; set; } = false;
}

public class PrescriptionBuilder
{
    private Prescription _prescription = new();

    public PrescriptionBuilder SetPatientInfo(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Patient name required");
        _prescription.PatientName = name;
        return this;
    }

    public PrescriptionBuilder AddMedication(string name, string dosage, string instructions)
    {
        _prescription.Medications.Add(new Medication { Name = name, Dosage = dosage, Instructions = instructions });
        return this;
    }

    public PrescriptionBuilder SignPrescription()
    {
        if (_prescription.Medications.Count == 0) throw new InvalidOperationException("Cannot sign empty prescription");
        _prescription.HasDoctorSignature = true;
        return this;
    }

    public Prescription Build()
    {
        if (!_prescription.HasDoctorSignature) throw new InvalidOperationException("Prescription must be signed before building.");
        var result = _prescription;
        _prescription = new(); // Reset
        return result;
    }
}

public class DoctorDirector
{
    private PrescriptionBuilder _builder;
    public DoctorDirector(PrescriptionBuilder builder) { _builder = builder; }
    
    public Prescription CreateAntibioticPrescription(string patientName, string antibioticName)
    {
        return _builder
            .SetPatientInfo(patientName)
            .AddMedication(antibioticName, "500mg", "Take one pill every 8 hours for 7 days")
            .AddMedication("Probiotic", "1 pill", "Take daily, 2 hours after antibiotic")
            .SignPrescription()
            .Build();
    }
}
