using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

[ApiController]
[Route("api/chatbot")]
public class ChatBotController : ControllerBase
{
    // Estados por usuário
    private static Dictionary<string, string> UserStates = new Dictionary<string, string>();

    [HttpPost]
    public IActionResult ReceiveMessage([FromBody] ChatMessage message)
    {
        var userId = message.Sender;

        // Inicializa o estado do usuário, se não existir
        if (!UserStates.ContainsKey(userId))
        {
            UserStates[userId] = "main_menu";
        }

        var userState = UserStates[userId];
        string botResponse;

        // Comando universal para retornar ao menu principal
        if (NormalizeString(message.Message) == "menu")
        {
            UserStates[userId] = "main_menu";
            botResponse = GetMainMenu();
        }
        else
        {
            // Processa o estado atual do usuário
            botResponse = HandleUserState(userState, message.Message, userId);
        }

        return Ok(new ChatMessage { Sender = "Bot", Message = botResponse });
    }

    private string HandleUserState(string state, string userMessage, string userId)
    {
        switch (state)
        {
            case "main_menu":
                return HandleMainMenu(userMessage, userId);

            case "specialties_menu":
                return HandleSpecialtiesMenu(userMessage, userId);

            case "treatments_menu":
                return HandleTreatmentsMenu(userMessage, userId);

            case "waiting_attendant":
                return HandleWaitingAttendant(userMessage, userId);

            default:
                // Retorna ao menu principal em caso de estado inválido
                UserStates[userId] = "main_menu";
                return GetMainMenu();
        }
    }

    private string HandleMainMenu(string userMessage, string userId)
    {
        string normalizedMessage = NormalizeString(userMessage);

        switch (normalizedMessage)
        {
            case "1":
                return "Os horários de funcionamento da clínica são de Segunda a Sexta, das 8h às 18h. Aos sábados, atendemos das 8h às 12h.";
            case "2":
                // Transição para o submenu de especialidades
                UserStates[userId] = "specialties_menu";
                return "Para agendar uma consulta, escolha a especialidade desejada:\n" +
                       "1 - Nutricionista\n" +
                       "2 - Ginecologista\n" +
                       "3 - Mastologista\n" +
                       "4 - Ortopedia\n" +
                       "5 - Cardiologia\n\n" +
                       "Digite o número correspondente para escolher a especialidade ou 'menu' para voltar ao menu inicial.";
            case "3":
                return "Estamos localizados na Rua Central, 123, Bairro Saúde. Venha nos visitar!";
            case "4":
                return "Aceitamos diversos convênios, incluindo Unimed, Bradesco Saúde e Amil.\n\nDigite 'menu' para voltar ao menu.";
            case "5":
                return "Realizamos diversos exames, como ultrassom, raio-x, exames laboratoriais e check-ups completos.\n\nDigite 'menu' para voltar ao menu.";
            case "6":
                // Transição para tratamentos e especialidades
                UserStates[userId] = "treatments_menu";
                return "Informações sobre tratamentos e especialidades:\n" +
                       "1 - Cardiologia\n" +
                       "2 - Dermatologia\n" +
                       "3 - Ortopedia\n\n" +
                       "Digite o número correspondente ou 'menu' para voltar ao menu principal.";
            case "7":
                // Transição para espera de atendente real
                UserStates[userId] = "waiting_attendant";
                return "Estamos chamando um atendente real para tirar suas dúvidas. Enquanto isso, posso ajudar com algo mais? Digite 'menu' para retornar ao menu principal.";
            default:
                return "Desculpe, não entendi sua resposta. Por favor, escolha uma opção válida ou digite 'menu' para voltar ao menu inicial.";
        }
    }

    private string HandleSpecialtiesMenu(string userMessage, string userId)
    {
        string normalizedMessage = NormalizeString(userMessage);

        switch (normalizedMessage)
        {
            case "1":
                UserStates[userId] = "main_menu"; // Reseta o estado após exibir a mensagem
                return "Para marcar uma consulta com um Nutricionista, ligue para nossa central de marcação: (XX) XXXX-XXXX.\n\nDigite 'menu' para voltar ao menu.";
            case "2":
                UserStates[userId] = "main_menu";
                return "Para marcar uma consulta com um Ginecologista, ligue para nossa central de marcação: (XX) XXXX-XXXX.\n\nDigite 'menu' para voltar ao menu.";
            case "3":
                UserStates[userId] = "main_menu";
                return "Para marcar uma consulta com um Mastologista, ligue para nossa central de marcação: (XX) XXXX-XXXX.\n\nDigite 'menu' para voltar ao menu.";
            case "4":
                UserStates[userId] = "main_menu";
                return "Para marcar uma consulta com um Ortopedista, ligue para nossa central de marcação: (XX) XXXX-XXXX.\n\nDigite 'menu' para voltar ao menu.";
            case "5":
                UserStates[userId] = "main_menu";
                return "Para marcar uma consulta com um Cardiologista, ligue para nossa central de marcação: (XX) XXXX-XXXX.\n\nDigite 'menu' para voltar ao menu.";
            default:
                return "Opção inválida. Digite 'menu' para voltar ao menu principal.";
        }
    }

    private string HandleTreatmentsMenu(string userMessage, string userId)
    {
        string normalizedMessage = NormalizeString(userMessage);

        switch (normalizedMessage)
        {
            case "1":
                UserStates[userId] = "main_menu";
                return "Na Cardiologia, realizamos exames de ecocardiograma, eletrocardiograma e mais.\n\nDigite 'menu' para voltar ao menu.";
            case "2":
                UserStates[userId] = "main_menu";
                return "Na Dermatologia, oferecemos tratamentos para acne, psoríase e exames preventivos.\n\nDigite 'menu' para voltar ao menu.";
            case "3":
                UserStates[userId] = "main_menu";
                return "Na Ortopedia, tratamos lesões musculares, fraturas e realizamos fisioterapia.\n\nDigite 'menu' para voltar ao menu.";
            default:
                return "Opção inválida. Digite 'menu' para voltar ao menu principal.";
        }
    }

    private string HandleWaitingAttendant(string userMessage, string userId)
    {
        return "Estamos chamando um atendente real. Enquanto isso, posso ajudar com algo mais? Digite 'menu' para retornar ao menu principal.";
    }

    private string GetMainMenu()
    {
        return "Como posso ajudar você? Escolha uma das opções abaixo e digite o número correspondente:<br>" +
            "1 - Ver horários de funcionamento<br>" +
            "2 - Como agendar uma consulta<br>" +
            "3 - Endereço da clínica<br>" +
            "4 - Convênios aceitos<br>" +
            "5 - Exames disponíveis<br>" +
            "6 - Informações sobre tratamentos e especialidades<br>" +
            "7 - Quero falar com atendente real<br>" +
            "<br>" +
            "Digite o número correspondente ou 'menu' para voltar ao início.";
    }

    private string NormalizeString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        return new string(input.ToLowerInvariant()
            .Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());
    }
}

public class ChatMessage
{
    public string Sender { get; set; }
    public string Message { get; set; }
}
