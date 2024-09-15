import { Paper, Stack } from "@mui/material";
import { FormProvider, useForm } from "react-hook-form";
import PaymentInfoForm from "./PaymentInfoForm";
import { SellerInfoForm } from "./SellerInfoForm";
import VehicleInfoForm from "./VehicleInfoForm";

export type PaymentMethod = "cash" | "bank_transfer";

export type FormValues = {
  // sellers info
  code: number | "";
  name: string;
  phone: string;
  email: string;
  // vehicle info
  sdk: string;
  make: string;
  registrationNumber: string;
  mileage: number | "";
  identificationNumber: string;
  serialNumber: number | "";
  technicalInspectionIsValid: boolean | "";
  incidents: boolean | "";
  knowAboutIncidents: boolean | "";
  defects: string[];
  // payment info
  price: number | "";
  paymentMethod: PaymentMethod | "";
  paymentOnTheSpot: boolean;
  paymentDate: string;
  buyersEmail: string;
};

export const PPForm = () => {
  const methods = useForm<FormValues>({
    defaultValues: {
      code: "",
      name: "",
      phone: "",
      email: "",
      sdk: "",
      make: "",
      registrationNumber: "",
      mileage: "",
      identificationNumber: "",
      serialNumber: "",
      technicalInspectionIsValid: "",
      incidents: "",
      knowAboutIncidents: "",
      defects: [],
      price: "",
      paymentMethod: "",
      paymentDate: "",
      buyersEmail: ""
    }
  });

  return (
    <FormProvider {...methods}>
      <Paper
        component="form"
        sx={{ maxWidth: "21cm", p: 3, mx: "auto", my: 3 }}
        elevation={4}
      >
        <Stack gap={3}>
          <SellerInfoForm />
          <VehicleInfoForm />
          <PaymentInfoForm />
        </Stack>
      </Paper>
    </FormProvider>
  );
};
