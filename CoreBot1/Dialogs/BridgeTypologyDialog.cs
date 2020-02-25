// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.6.2

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBot1.Dialogs
{
    public class BridgeTypologyDialog : CancelAndHelpDialog
    {
        private const string DestinationStepMsgText = "Where do you want to design a bridge?";
        private const string MaterialStepMsgText = "What material is your bridge?";
        private const string SpanStepMsgText = "What span is your bridge?";

        //private const string OriginStepMsgText = "Where are you traveling from?";

        public BridgeTypologyDialog()
            : base(nameof(BridgeTypologyDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                DestinationStepAsync,
                MaterialStepAsync,
                SpanStepAsync,
                //OriginStepAsync,
                //TravelDateStepAsync,
                ConfirmStepAsync,
                //FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> DestinationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var bookingDetails = (BridgeTopologyDetails)stepContext.Options;
            if (bookingDetails.Country == null)
            {
                var promptMessage = MessageFactory.Text(DestinationStepMsgText, DestinationStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookingDetails.Country, cancellationToken);
        }

        private async Task<DialogTurnResult> MaterialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var bookingDetails = (BridgeTopologyDetails)stepContext.Options;
            bookingDetails.Country = (string)stepContext.Result;

            if (bookingDetails.Material == null)
            {
                var promptMessage = MessageFactory.Text(MaterialStepMsgText, MaterialStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                
            }

            return await stepContext.NextAsync(bookingDetails.Material, cancellationToken);
        }

        private async Task<DialogTurnResult> SpanStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            var bookingDetails = (BridgeTopologyDetails)stepContext.Options;
            bookingDetails.Material = (string)stepContext.Result;

            if (bookingDetails.MaxSpan == null)
            {
                var promptMessage = MessageFactory.Text(SpanStepMsgText, SpanStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                //var promptMessage = MessageFactory.Text(DestinationStepMsgText, DestinationStepMsgText, InputHints.ExpectingInput);
                //return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookingDetails.MaxSpan, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (BridgeTopologyDetails)stepContext.Options;
            bookingDetails.MaxSpan = (string)stepContext.Result;

            //bookingDetails.TravelDate = (string)stepContext.Result;

            var messageText = $"For a {bookingDetails.Material} bridge in {bookingDetails.Country} with a span of {bookingDetails.MaxSpan}, I recommend the following systems: ";
            messageText += " Prestressed concrete beam bridge";
            //For a Concrete bridge in Germany with a span of 18m, I recommend the following systems: Prestressed concrete beam bridge.
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
            //return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            return await stepContext.EndDialogAsync(bookingDetails, cancellationToken);
            //return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);

            //var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            //messageText
            //var getPersonFromProjectMessage = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);

        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var bookingDetails = (BridgeTopologyDetails)stepContext.Options;
                
                return await stepContext.EndDialogAsync(bookingDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        //private static bool IsAmbiguous(string timex)
        //{
        //    var timexProperty = new TimexProperty(timex);
        //    return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        //}
    }
}
