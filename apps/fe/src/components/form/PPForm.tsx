import { yupResolver } from "@hookform/resolvers/yup";
import { Button, Paper, Stack } from "@mui/material";
import { FormProvider, useForm } from "react-hook-form";
import PaymentInfoForm from "./PaymentInfoForm";
import { SellerInfoForm } from "./SellerInfoForm";
import VehicleInfoForm from "./VehicleInfoForm";
import { formSchema } from "./validation";

export type PaymentMethod = "cash" | "bank_transfer";

type FormBoolean = "true" | "false" | "";

export type FormValues = {
  // sellers info
  personalCode: string;
  name: string;
  phone: string;
  sellersEmail: string;
  sellersAddress: string;
  // vehicle info
  sdk: string;
  make: string;
  registrationNumber: string;
  mileage: string;
  identificationNumber: string;
  serialNumber: string;
  technicalInspectionIsValid: FormBoolean;
  incidents: FormBoolean;
  knowAboutIncidents: FormBoolean;
  defects: string[];
  incidentsInformation: string;
  // payment info
  price: string;
  paymentMethod: PaymentMethod | "";
  paymentDate: string;
  buyersEmail: string;
};

export const PPForm = () => {
  const methods = useForm<FormValues>({
    defaultValues: {
      personalCode: "",
      name: "",
      phone: "",
      sellersEmail: "",
      sellersAddress: "",
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
      incidentsInformation: "",
      price: "",
      paymentMethod: "",
      paymentDate: new Date().toISOString(),
      buyersEmail: ""
    },
    resolver: yupResolver(formSchema)
  });

  const { handleSubmit } = methods;

  const onSubmit = handleSubmit((values: FormValues) => {
    // eslint-disable-next-line no-console
    console.log(values);
  });

  return (
    <FormProvider {...methods}>
      <Paper
        component="form"
        onSubmit={onSubmit}
        sx={{ maxWidth: "21cm", p: 3, mx: "auto", my: 3 }}
        elevation={4}
      >
        <Stack gap={3}>
          <SellerInfoForm />
          <VehicleInfoForm />
          <PaymentInfoForm />
          <Button type="submit" variant="contained">
            What a save!!!
          </Button>
        </Stack>
      </Paper>
    </FormProvider>
  );
};
