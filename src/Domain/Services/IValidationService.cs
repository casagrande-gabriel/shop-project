using Domain.ValueObjects;

namespace Domain.Services;

public interface IValidationService
{
    void ValidateAddress(Address address);
    bool ValidateCPF(string cpf);
    void ValidateEmail(string email);
    void ValidateTelephone(string telephone);
}