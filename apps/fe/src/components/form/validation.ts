import * as yup from "yup";

const requiredText = "☝️ Privalomas laukas";

export const formSchema = yup.object({
  //Seller info
  personalCode: yup
    .string()
    .required(requiredText)
    .matches(/^\d{11}$/, "☝️ Asmens kodą turi sudaryti 11 skaitmenų"),
  name: yup
    .string()
    .required(requiredText)
    .min(3, "☝️ Vardą turi sudaryti mažiausiai 3 simboliai")
    .matches(/^[^\s]+(\s+)[^\s]+$/, "☝️ Privalomas tarpas"),
  phone: yup
    .string()
    .required(requiredText)
    .matches(/^\d{6}$/, "☝️ Telefono numerį turi sudaryti 6 skaitmenys"),
  sellersEmail: yup
    .string()
    .email("☝️ Netinkamas el. pašto formatas")
    .required(requiredText),
  sellersAddress: yup.string().required(requiredText),
  // Vehicle info
  sdk: yup
    .string()
    .required(requiredText)
    .matches(/^[A-Za-z]{8}$/, "☝️ SDK turi būti 8 raidės"),
  make: yup.string().required(requiredText),
  registrationNumber: yup
    .string()
    .required(requiredText)
    .matches(
      /^(E[A-Za-z]{2}[- ]?\d{4}|[A-Za-z]{3}[- ]?\d{3}|[e][a]{2}\d{4})$/i,
      "☝️ Netinkamas numerio formatas"
    ),
  mileage: yup
    .string()
    .required(requiredText)
    .matches(/\d/, "☝️ Ridą turi sudaryti skaičiai"),
  identificationNumber: yup
    .string()
    .required(requiredText)
    .matches(
      /^[A-Za-z0-9]{17}$/,
      "☝️ Identifikacijos numerį turi sudaryti 17 simbolių"
    ),
  serialNumber: yup
    .string()
    .required(requiredText)
    .matches(/\d/, "☝️ Registracijos liudijimo numerį turi sudaryti skaičiai"),
  technicalInspectionIsValid: yup
    .boolean()
    .required(requiredText)
    .typeError(requiredText),
  incidents: yup.boolean().required(requiredText).typeError(requiredText),
  knowAboutIncidents: yup
    .boolean()
    .required(requiredText)
    .typeError(requiredText),
  // Payment info
  price: yup
    .string()
    .required(requiredText)
    .matches(/\d/, "☝️ Kainą turi sudaryti skaičiai"),
  paymentMethod: yup.string().required(requiredText),
  buyersEmail: yup
    .string()
    .required(requiredText)
    .email("☝️ Netinkamas el. pašto formatas")
});
