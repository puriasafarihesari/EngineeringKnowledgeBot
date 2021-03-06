// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.6.2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

using CoreBot1.CognitiveModels;

namespace CoreBot1.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly FlightBookingRecognizer _luisRecognizer;
        protected readonly ILogger Logger;
        private int _askCounter = 0;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(FlightBookingRecognizer luisRecognizer, BookingDialog bookingDialog, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;
            _askCounter = 0;

            ReadDataBase.ReadData();
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(bookingDialog);
            AddDialog(new BridgeTypologyDialog());
            AddDialog(new CreatePostDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                await stepContext.Context.SendActivityAsync(
                    MessageFactory.Text("NOTE: LUIS is not configured. To enable all capabilities, add 'LuisAppId', 'LuisAPIKey' and 'LuisAPIHostName' to the appsettings.json file.", inputHint: InputHints.IgnoringInput), cancellationToken);

                return await stepContext.NextAsync(null, cancellationToken);
            }

            // Use the text provided in FinalStepAsync or the default if it is the first time.
            var messageText = stepContext.Options?.ToString() ?? "What can I help you with?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions(), cancellationToken);
        }
        

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (!_luisRecognizer.IsConfigured)
            {
                // LUIS is not configured, we just run the BookingDialog path with an empty BookingDetailsInstance.
                return await stepContext.BeginDialogAsync(nameof(BookingDialog), new BookingDetails(), cancellationToken);
            }

            // Call LUIS and gather any potential booking details. (Note the TurnContext has the response to the prompt.)
            var luisResult = await _luisRecognizer.RecognizeAsync<FlightBooking>(stepContext.Context, cancellationToken);
            switch (luisResult.TopIntent().intent)
            {
                case FlightBooking.Intent.BookFlight:
                    //await ShowWarningForUnsupportedCities(stepContext.Context, luisResult, cancellationToken);

                    // Initialize BookingDetails with any entities we may have found in the response.
                    var bookingDetails = new BookingDetails()
                    {
                        // Get destination and origin from the composite entities arrays.
                        Destination = luisResult.ToEntities.Airport,
                        Origin = luisResult.FromEntities.Airport,
                        TravelDate = luisResult.TravelDate,
                    };

                    // Run the BookingDialog giving it whatever details we have from the LUIS call, it will fill out the remainder.
                    return await stepContext.BeginDialogAsync(nameof(BookingDialog), bookingDetails, cancellationToken);

                case FlightBooking.Intent.GetWeather:
                    
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var getWeatherMessageText = "TODO: get weather flow here";
                    var getWeatherMessage = MessageFactory.Text(getWeatherMessageText, getWeatherMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getWeatherMessage, cancellationToken);
                    break;
                case FlightBooking.Intent.GetBridgeTypology:
                    BridgeTopologyDetails topologyDetails = new BridgeTopologyDetails()
                    {
                        Country = luisResult.Country,
                        Material = luisResult.Material,
                        MaxSpan = luisResult.MaxSpan
                    };
                    return await stepContext.BeginDialogAsync(nameof(BridgeTypologyDialog), topologyDetails, cancellationToken);
                //var getBridgeMessageText = "Sick dude, what kind of bridge?";
                //var GgtBridgeMessageMessage = MessageFactory.Text(getBridgeMessageText, getBridgeMessageText, InputHints.IgnoringInput);
                //await stepContext.Context.SendActivityAsync(GgtBridgeMessageMessage, cancellationToken);
                case FlightBooking.Intent.GetPersonFromSkill:
                    var skill = luisResult.Skill;
                    
                    var getPersonFromSkillMessageText = string.Empty;
                    try
                    {
                        var foundPerson = GetDataFromDB.FindPersonWithSkill(skill.ToLower());
                        getPersonFromSkillMessageText = $"The people below are great at {skill}:\r\n{foundPerson}";
                    }
                    catch (Exception e)
                    {
                        getPersonFromSkillMessageText = e.ToString();
                        //throw;
                    }
                    var getPersonFromSkillMessage = MessageFactory.Text(getPersonFromSkillMessageText, getPersonFromSkillMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getPersonFromSkillMessage, cancellationToken);
                    break;
                case FlightBooking.Intent.GetTranslation:
                    var word = luisResult.Word;
                    var language = luisResult.Language;
                    var translatedWord = GetDataFromDB.FindWord(word.ToLower(), language);
                    var getTranslatedWordMessageText = $"The {language} word for {word} is {translatedWord}";
                    var getTranslatedWordMessage = MessageFactory.Text(getTranslatedWordMessageText, getTranslatedWordMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getTranslatedWordMessage, cancellationToken);
                    break;
                case FlightBooking.Intent.GetProjectFromTypology:
                    string getProjectFromTypoMessageText = string.Empty;
                    try
                    {
                        var typologi = luisResult.Typology;
                        if (typologi != null)
                        {
                            var matchedProjects = GetDataFromDB.FindProjcetByTypology(typologi.ToLower());
                            getProjectFromTypoMessageText = $"The projects below are using the same typology as {typologi}:\r\n{matchedProjects}";
                        }
                        else
                        {
                            getProjectFromTypoMessageText = "Try again.";
                        }

                    }
                    catch (Exception e)
                    {
                        getProjectFromTypoMessageText = e.ToString();
                        //throw ;
                    }
                    var getProjectFromTypoMessage = MessageFactory.Text(getProjectFromTypoMessageText, getProjectFromTypoMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getProjectFromTypoMessage, cancellationToken);
                    break;
                case FlightBooking.Intent.GetPersonFromProject:
                    var project = luisResult.Project;
                    var people = "bunch of people";
                    var getPersonFromProjectMessageText = $"{project} is great at {people}";
                    var getPersonFromProjectMessage = MessageFactory.Text(getPersonFromProjectMessageText, getPersonFromProjectMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(getPersonFromProjectMessage, cancellationToken);
                    break;
                case FlightBooking.Intent.ShowProject:
                    var projectModel = luisResult.Project;
                    string url = StreamProject.GetProjectUrl(projectModel);
                    string response = string.Empty;
                    if(url == null)
                    {
                        response = $"sorry couldn't find that model.";
                    }
                    else
                    {
                        response = $"A 3d view of the model '{projectModel}' can be found here: ";
                        response += url;
                    }
                    var msgMessage1 = MessageFactory.Text(response, response, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(msgMessage1, cancellationToken);
                    break;
                case FlightBooking.Intent.GenerateParametricBuilding:
                    var curviness = luisResult.Curviness;
                    var levels = luisResult.Levels;
                    StreamProject.StreamBuilding(levels, curviness);
                    string msg = $"I have generated a building with {levels} levels. Please see the link here: ";
                    msg += "http://bot.continuum.codes/viewer.html";
                    var msgMessage = MessageFactory.Text(msg, msg, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(msgMessage, cancellationToken);
                    break;
                case FlightBooking.Intent.WhatsNext:
                    var whatsnext = "Thank you! I will continue to learn from all of you. Your quesions will help me grow my knowledge such that I can better support you in the future. A first step is to run micro - applications to help you solve your problems and, as my database grows, I will be able to suggest solutions to increasingly complex technical problems.";
                    //var matchedProjects = GetDataFromDB.FindProjcetByTypology(typologi.ToLower());
                    //var getProjectFromTypoMessageText = $"The projects below are using the same typology as {typologi}:\r\n{matchedProjects}";
                    var whatsnextMsg = MessageFactory.Text(whatsnext, whatsnext, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(whatsnextMsg, cancellationToken);
                    break;
                default:
                    var act = stepContext?.Context?.Activity?.Text;
                    if (_askCounter > 0 && act != "Hello")
                    {
                        return await stepContext.BeginDialogAsync(nameof(CreatePostDialog), null, cancellationToken);
                    }

                    break;
                    // Catch all for unhandled intents
                    //var didntUnderstandMessageText = $"Sorry, I didn't get that. Please try asking in a different way (intent was {luisResult.TopIntent().intent})";
                    //var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    // await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    //break;
            }
            _askCounter++;
            return await stepContext.NextAsync(null, cancellationToken);
        }

        // Shows a warning if the requested From or To cities are recognized as entities but they are not in the Airport entity list.
        // In some cases LUIS will recognize the From and To composite entities as a valid cities but the From and To Airport values
        // will be empty if those entity values can't be mapped to a canonical item in the Airport.
        private static async Task ShowWarningForUnsupportedCities(ITurnContext context, FlightBooking luisResult, CancellationToken cancellationToken)
        {
            var unsupportedCities = new List<string>();

            var fromEntities = luisResult.FromEntities;
            if (!string.IsNullOrEmpty(fromEntities.From) && string.IsNullOrEmpty(fromEntities.Airport))
            {
                unsupportedCities.Add(fromEntities.From);
            }

            var toEntities = luisResult.ToEntities;
            if (!string.IsNullOrEmpty(toEntities.To) && string.IsNullOrEmpty(toEntities.Airport))
            {
                unsupportedCities.Add(toEntities.To);
            }

            if (unsupportedCities.Any())
            {
                var messageText = $"Sorry but the following airports are not supported: {string.Join(',', unsupportedCities)}";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await context.SendActivityAsync(message, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is BookingDetails result)
            {
                // Now we have all the booking details call the booking service.

                // If the call to the booking service was successful tell the user.

                var timeProperty = new TimexProperty(result.TravelDate);
                var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
                var messageText = $"I have you booked to {result.Destination} from {result.Origin} on {travelDateMsg}";
                var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }
            if (_askCounter > 1)
            {
                var promptMessage = "What else can I do for you?";
                return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
            }
            else
            {
                return await stepContext.NextAsync(null, cancellationToken);
            }
            // Restart the main dialog with a different message the second time around
        }
    }
}
