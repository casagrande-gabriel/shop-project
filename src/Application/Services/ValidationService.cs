using System.Text.RegularExpressions;
using Domain.Services;
using Domain.ValueObjects;

namespace Projeto.Services;

public partial class ValidationService : IValidationService
{
    [GeneratedRegex(@"[\w.]+@\w+\.\w+")]
    private partial Regex EmailRegex();

    [GeneratedRegex(@"(\(\d{2}\))?\s?(\d{4,5})-?(\d{4})")]
    private partial Regex TelephoneRegex();

    public bool ValidateCPF(string cpf)
    {
        int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digit;
        int sum;
        int rest;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11) return false;
        tempCpf = cpf.Substring(0, 9);
        sum = 0;

        for (int i = 0; i < 9; i++) sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        rest = sum % 11;

        if (rest < 2) rest = 0;
        else rest = 11 - rest;

        digit = rest.ToString();
        tempCpf += digit;
        sum = 0;

        for (int i = 0; i < 10; i++) sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        rest = sum % 11;

        if (rest < 2) rest = 0;
        else rest = 11 - rest;

        digit += rest.ToString();

        return cpf.EndsWith(digit);
    }

    public void ValidateAddress(Address address)
    {
        if (string.IsNullOrEmpty(address.Street))
        {
            throw new ArgumentException("O campo \"Rua\" do endereço deve ser informado.");
        }

        if (string.IsNullOrEmpty(address.Number))
        {
            throw new ArgumentException("O campo \"Número\" do endereço deve ser informado.");
        }

        if (string.IsNullOrEmpty(address.Neighborhood))
        {
            throw new ArgumentException("O campo \"Bairro\" do endereço deve ser informado.");
        }

        if (string.IsNullOrEmpty(address.ZipCode))
        {
            throw new ArgumentException("O campo \"CEP\" do endereço deve ser informado.");

        }

        if (string.IsNullOrEmpty(address.City))
        {
            throw new ArgumentException("O campo \"Cidade\" do endereço deve ser informado.");
        }

        if (string.IsNullOrEmpty(address.State))
        {
            throw new ArgumentException("O campo \"Estado\" do endereço deve ser informado.");
        }
    }

    public void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("O e-mail não pode ser vazio.");
        }

        if (!EmailRegex().IsMatch(email))
        {
            throw new ArgumentException("O e-mail informado não é válido.");
        }
    }

    public void ValidateTelephone(string telephone)
    {
        if (string.IsNullOrEmpty(telephone))
        {
            throw new ArgumentException("O telefone não pode ser vazio.");
        }

        if (!TelephoneRegex().IsMatch(telephone))
        {
            throw new ArgumentException("O telefone informado não é válido.");
        }
    }
}