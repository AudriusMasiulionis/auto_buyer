namespace AutoDokas.Extensions;

public static class NumberToWordsExtensions
{
    private static readonly string[] Units =
    [
        "", "vienas", "du", "trys", "keturi", "penki", "šeši", "septyni", "aštuoni", "devyni"
    ];

    private static readonly string[] Teens =
    [
        "dešimt", "vienuolika", "dvylika", "trylika", "keturiolika", "penkiolika",
        "šešiolika", "septyniolika", "aštuoniolika", "devyniolika"
    ];

    private static readonly string[] Tens =
    [
        "", "", "dvidešimt", "trisdešimt", "keturiasdešimt", "penkiasdešimt",
        "šešiasdešimt", "septyniasdešimt", "aštuoniasdešimt", "devyniasdešimt"
    ];

    private static readonly string[] Hundreds =
    [
        "", "vienas šimtas", "du šimtai", "trys šimtai", "keturi šimtai", "penki šimtai",
        "šeši šimtai", "septyni šimtai", "aštuoni šimtai", "devyni šimtai"
    ];

    public static string ToLithuanianWords(this decimal amount)
    {
        var euros = (long)Math.Floor(amount);
        var cents = (int)Math.Round((amount - euros) * 100);

        var result = ConvertToWords(euros);

        // Add euro word with correct form
        result += " " + GetEuroForm(euros);

        if (cents > 0)
        {
            result += " " + ConvertToWords(cents) + " " + GetCentForm(cents);
        }

        // Capitalize first letter
        if (result.Length > 0)
        {
            result = char.ToUpper(result[0]) + result[1..];
        }

        return result.Trim();
    }

    private static string ConvertToWords(long number)
    {
        if (number == 0)
            return "nulis";

        if (number < 0)
            return "minus " + ConvertToWords(-number);

        var words = "";

        if (number >= 1_000_000_000)
        {
            var billions = number / 1_000_000_000;
            words += ConvertToWords(billions) + " " + GetBillionForm(billions) + " ";
            number %= 1_000_000_000;
        }

        if (number >= 1_000_000)
        {
            var millions = number / 1_000_000;
            words += ConvertToWords(millions) + " " + GetMillionForm(millions) + " ";
            number %= 1_000_000;
        }

        if (number >= 1000)
        {
            var thousands = number / 1000;
            words += GetThousandWords(thousands) + " ";
            number %= 1000;
        }

        if (number >= 100)
        {
            words += Hundreds[number / 100] + " ";
            number %= 100;
        }

        if (number >= 20)
        {
            words += Tens[number / 10] + " ";
            number %= 10;
        }

        if (number >= 10)
        {
            words += Teens[number - 10] + " ";
            number = 0;
        }

        if (number > 0)
        {
            words += Units[number] + " ";
        }

        return words.Trim();
    }

    private static string GetThousandWords(long thousands)
    {
        if (thousands == 1)
            return "vienas tūkstantis";

        var form = GetThousandForm(thousands);
        return ConvertToWords(thousands) + " " + form;
    }

    private static string GetThousandForm(long n)
    {
        if (n % 10 == 1 && n % 100 != 11)
            return "tūkstantis";
        if (n % 10 >= 2 && n % 10 <= 9 && (n % 100 < 10 || n % 100 >= 20))
            return "tūkstančiai";
        return "tūkstančių";
    }

    private static string GetMillionForm(long n)
    {
        if (n % 10 == 1 && n % 100 != 11)
            return "milijonas";
        if (n % 10 >= 2 && n % 10 <= 9 && (n % 100 < 10 || n % 100 >= 20))
            return "milijonai";
        return "milijonų";
    }

    private static string GetBillionForm(long n)
    {
        if (n % 10 == 1 && n % 100 != 11)
            return "milijardas";
        if (n % 10 >= 2 && n % 10 <= 9 && (n % 100 < 10 || n % 100 >= 20))
            return "milijardai";
        return "milijardų";
    }

    private static string GetEuroForm(long n)
    {
        if (n % 10 == 1 && n % 100 != 11)
            return "euras";
        if (n % 10 >= 2 && n % 10 <= 9 && (n % 100 < 10 || n % 100 >= 20))
            return "eurai";
        return "eurų";
    }

    private static string GetCentForm(long n)
    {
        if (n % 10 == 1 && n % 100 != 11)
            return "centas";
        if (n % 10 >= 2 && n % 10 <= 9 && (n % 100 < 10 || n % 100 >= 20))
            return "centai";
        return "centų";
    }
}
