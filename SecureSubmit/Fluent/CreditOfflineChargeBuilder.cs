﻿using Hps.Exchange.PosGateway.Client;
using SecureSubmit.Abstractions;
using SecureSubmit.Entities;
using SecureSubmit.Entities.Credit;
using SecureSubmit.Fluent.Services;
using SecureSubmit.Infrastructure.Validation;

namespace SecureSubmit.Fluent {
    public class CreditOfflineChargeBuilder : HpsBuilderAbstract<HpsFluentCreditService, HpsTransaction> {
        decimal? amount;
        HpsCreditCard card;
        string token;
        HpsTrackData trackData;
        HpsCardHolder cardHolder;
        bool requestMultiUseToken = false;
        HpsTransactionDetails details;
        string txnDescriptor;
        bool cpcReq = false;
        HpsDirectMarketData directMarketData;
        bool allowDuplicates = false;
        bool cardPresent = false;
        bool readerPresent = false;
        decimal? gratuity;
        HpsAutoSubstantiation autoSubstantiation;
        string offlineAuthCode;
        EMVDataType emvData;

        public CreditOfflineChargeBuilder WithAmount(decimal? amount) {
            this.amount = amount;
            return this;
        }
        public CreditOfflineChargeBuilder WithCard(HpsCreditCard card) {
            this.card = card;
            return this;
        }
        public CreditOfflineChargeBuilder WithToken(string token) {
            this.token = token;
            return this;
        }
        public CreditOfflineChargeBuilder WithTrackData(HpsTrackData trackData) {
            this.trackData = trackData;
            return this;
        }
        public CreditOfflineChargeBuilder WithEMVData(EMVDataType emvData) {
            this.emvData = emvData;
            return this;
        }
        public CreditOfflineChargeBuilder WithCardHolder(HpsCardHolder cardHolder) {
            this.cardHolder = cardHolder;
            return this;
        }
        public CreditOfflineChargeBuilder WithRequestMultiUseToken(bool requestMultiUseToken) {
            this.requestMultiUseToken = requestMultiUseToken;
            return this;
        }
        public CreditOfflineChargeBuilder WithDetails(HpsTransactionDetails details) {
            this.details = details;
            return this;
        }
        public CreditOfflineChargeBuilder WithTxnDescriptor(string txnDescriptor) {
            this.txnDescriptor = txnDescriptor;
            return this;
        }
        public CreditOfflineChargeBuilder WithCpcReq(bool cpcReq) {
            this.cpcReq = cpcReq;
            return this;
        }
        public CreditOfflineChargeBuilder WithAllowDuplicates(bool allowDuplicates) {
            this.allowDuplicates = allowDuplicates;
            return this;
        }
        public CreditOfflineChargeBuilder WithCardPresent(bool cardPresent) {
            this.cardPresent = cardPresent;
            return this;
        }
        public CreditOfflineChargeBuilder WithReaderPresent(bool readerPresent) {
            this.readerPresent = readerPresent;
            return this;
        }
        public CreditOfflineChargeBuilder WithGratuity(decimal? gratuity) {
            this.gratuity = gratuity;
            return this;
        }
        public CreditOfflineChargeBuilder WithAutoSubstantiation(HpsAutoSubstantiation autoSubstantiation) {
            this.autoSubstantiation = autoSubstantiation;
            return this;
        }
        public CreditOfflineChargeBuilder WithOfflineAuthCode(string code) {
            this.offlineAuthCode = code;
            return this;
        }
        public CreditOfflineChargeBuilder WithDirectMarketData(HpsDirectMarketData directMarketData) {
            this.directMarketData = directMarketData;
            return this;
        }

        public CreditOfflineChargeBuilder(HpsFluentCreditService service)
            : base(service) {
        }

        public override HpsTransaction Execute() {
            base.Execute();

            var block1 = new CreditOfflineSaleReqBlock1Type {
                AllowDup = allowDuplicates ? booleanType.Y : booleanType.N,
                AllowDupSpecified = true,
                Amt = amount.Value
            };

            block1.GratuityAmtInfoSpecified = gratuity.HasValue;
            if (block1.GratuityAmtInfoSpecified)
                block1.GratuityAmtInfo = gratuity.Value;

            if (cardHolder != null)
                block1.CardHolderData = service.HydrateCardHolderData(cardHolder);

            var cardData = new CardDataType();
            if (card != null) {
                cardData.Item = service.HydrateCardManualEntry(card, cardPresent, readerPresent);
                if (card.EncryptionData != null)
                    cardData.EncryptionData = service.HydrateEncryptionData(card.EncryptionData);
            }
            if (token != null)
                cardData.Item = service.HydrateTokenData(token, cardPresent, readerPresent);
            if (trackData != null) {
                cardData.Item = service.HydrateCardTrackData(trackData);
                if (trackData.EncryptionData != null)
                    cardData.EncryptionData = service.HydrateEncryptionData(trackData.EncryptionData);
            }
            cardData.TokenRequest = requestMultiUseToken ? booleanType.Y : booleanType.N;
            block1.CardData = cardData;

            if (cpcReq) {
                block1.CPCReq = booleanType.Y;
                block1.CPCReqSpecified = true;
            }

            if (details != null)
                block1.AdditionalTxnFields = service.HydrateAdditionalTxnFields(details);
            if (autoSubstantiation != null)
                block1.AutoSubstantiation = service.HydrateAutoSubstantiation(autoSubstantiation);
            if (directMarketData != null)
                block1.DirectMktData = service.HydrateDirectMktData(directMarketData);
            block1.OfflineAuthCode = offlineAuthCode;

            if (emvData != null)
                block1.EMVTagData = emvData.EMVTagData;

            var transaction = new PosRequestVer10Transaction {
                Item = new PosCreditOfflineSaleReqType {
                    Block1 = block1
                },
                ItemElementName = ItemChoiceType1.CreditOfflineSale
            };

            var clientTransactionId = service.GetClientTransactionId(details);
            var response = service.SubmitTransaction(transaction, clientTransactionId);
            HpsTransaction trans = new HpsTransaction().FromResponse(response);
            trans.ResponseCode = "00";
            trans.ResponseText = string.Empty;
            return trans;
        }

        protected override void SetupValidations() {
            AddValidation(() => { return amount.HasValue; }, "Amount is required.");
            AddValidation(OnlyOnePaymentMethod, "Only one payment method is required.");
            AddValidation(() => { return (offlineAuthCode != null) || ((offlineAuthCode == null) && (emvData.EMVTagData.Length > 0)); }, "Offline auth code is required.");
        }

        private bool OnlyOnePaymentMethod() {
            int count = 0;
            if (card != null) count++;
            if (trackData != null) count++;
            if (token != null) count++;

            return count == 1;
        }
    }

}
