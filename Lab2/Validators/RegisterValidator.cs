using Lab2.DTOs;
using Lab2.Models;
using Lab2.Servies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Validators

{
    public interface IRegisterValidator
    {
        ErrorsCollection Validate(RegisterUserPostDto registerUserPostDto, ExpensesDbContext context);
    }

    public class RegisterValidator : IRegisterValidator
    {
        public ErrorsCollection Validate(RegisterUserPostDto registerUserPostDto, ExpensesDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(RegisterUserPostDto) };
            User existing = context.Users.FirstOrDefault(u => u.Username == registerUserPostDto.Username);
            if (existing != null)
            {
            
                errorsCollection.ErrorMessages.Add($"The username {registerUserPostDto.Username} is already taken!");
            }
            if (registerUserPostDto.Password.Length < 6)
            {
                errorsCollection.ErrorMessages.Add("The password cannot be shorter than 6 characters!");
            }
            int numberOfDigits = 0;
            foreach (char c in registerUserPostDto.Password)
            {
                if (c >= '0' && c <= '9')
                {
                    numberOfDigits++;
                }
            }
            if (numberOfDigits < 2)
            {
                errorsCollection.ErrorMessages.Add("The password must contain at least two digits!");
            }
            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;
        }

    }
}