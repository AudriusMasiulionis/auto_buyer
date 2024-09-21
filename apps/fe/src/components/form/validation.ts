import * as yup from "yup";

export const formSchema = yup.object({
  //Seller info
  personalCode: yup
    .string()
    .required("☝️ Privalomas laukas")
    .matches(/^\d{11}$/, "☝️ Asmens kodą turi sudaryti 11 skaitmenų"),
  name: yup
    .string()
    .required("☝️ Privalomas laukas")
    .min(3, "☝️ Vardą turi sudaryti mažiausiai 3 simboliai")
    .matches(/^[^\s]+(\s+)[^\s]+$/, "☝️ Privalomas tarpas"),
  phone: yup
    .string()
    .required("☝️ Privalomas laukas")
    .matches(/^\d{6}$/, "☝️ Telefono numerį turi sudaryti 6 skaitmenys"),
  sellersEmail: yup
    .string()
    .email("☝️ Netinkamas el. pašto formatas")
    .required("☝️ Privalomas laukas"),
  sellersAddress: yup.string().required("☝️ Privalomas laukas"),
  // Vehicle info
  sdk: yup
    .string()
    .required("☝️ Privalomas laukas")
    .matches(/^[A-Za-z]{8}$/, "☝️ SDK turi būti 8 raidės"),
  make: yup.string().required("☝️ Privalomas laukas"),
  registrationNumber: yup
    .string()
    .required("☝️ Privalomas laukas")
    .matches(
      /^(E[A-Za-z]{2}[- ]?\d{4}|[A-Za-z]{3}[- ]?\d{3}|[e][a]{2}\d{4})$/i,
      "☝️ Netinkamas numerio formatas"
    ),
  mileage: yup
    .string()
    .required("☝️ Privalomas laukas")
    .matches(/\d/, "☝️ Ridą turi sudaryti skaičiai"),
  identificationNumber: yup
    .string()
    .required("☝️ Privalomas laukas")
    .matches(
      /^[A-Za-z0-9]{17}$/,
      "☝️ Identifikacijos numerį turi sudaryti 17 simbolių"
    ),
  serialNumber: yup
    .string()
    .required("☝️ Privalomas laukas")
    .matches(/\d/, "☝️ Registracijos liudijimo numerį turi sudaryti skaičiai"),
  technicalInspectionIsValid: yup
    .boolean()
    .required("☝️ Privalomas laukas")
    .typeError("☝️ Privalomas laukas"),
  incidents: yup
    .boolean()
    .required("☝️ Privalomas laukas")
    .typeError("☝️ Privalomas laukas"),
  knowAboutIncidents: yup
    .boolean()
    .required("☝️ Privalomas laukas")
    .typeError("☝️ Privalomas laukas"),
  // Payment info
  price: yup
    .string()
    .required("☝️ Privalomas laukas")
    .matches(/\d/, "☝️ Kainą turi sudaryti skaičiai"),
  paymentMethod: yup.string().required("☝️ Privalomas laukas"),
  buyersEmail: yup
    .string()
    .required("☝️ Privalomas laukas")
    .email("☝️ Netinkamas el. pašto formatas")
});
