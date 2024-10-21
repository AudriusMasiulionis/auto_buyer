"use client";

import { yupResolver } from "@hookform/resolvers/yup";
import {
  Button,
  Paper,
  Stack,
  Step,
  StepLabel,
  Stepper,
  MobileStepper,
  useMediaQuery,
  useTheme
} from "@mui/material";
import { useState } from "react";
import { FormProvider, useForm } from "react-hook-form";
import * as yup from "yup";
import PaymentInfoForm from "./PaymentInfoForm";
import { SellerInfoForm } from "./SellerInfoForm";
import VehicleInfoForm from "./VehicleInfoForm";
import { formSchema } from "./validation";

export type FormValues = {
  personalCode: string;
  name: string;
  phone: string;
  sellersEmail: string;
  sellersAddress: string;
  sdk: string;
  make: string;
  registrationNumber: string;
  mileage: string;
  identificationNumber: string;
  serialNumber: string;
  technicalInspectionIsValid: boolean | "";
  incidents: boolean | "";
  knowAboutIncidents: boolean | "";
  defects: string[];
  incidentsInformation: string;
  price: string;
  paymentMethod: "cash" | "bank_transfer" | "";
  paymentDate: string;
  buyersEmail: string;
};

const fieldsToValidate: (keyof FormValues)[][] = [
  ["personalCode", "name", "phone", "sellersEmail", "sellersAddress"],
  [
    "sdk",
    "make",
    "registrationNumber",
    "mileage",
    "identificationNumber",
    "serialNumber",
    "technicalInspectionIsValid",
    "incidents"
  ],
  ["price", "paymentMethod", "paymentDate", "buyersEmail"]
];

const steps = [
  "Pardavėjo informacija",
  "Transporto priemonės informacija",
  "Atsiskaitymo informacija"
];

export const PPForm = () => {
  const [activeStep, setActiveStep] = useState(0);
  const theme = useTheme();
  const isMobile =  (theme.breakpoints.down("sm")); // Check if the view is mobile

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
    resolver: yupResolver(formSchema as yup.ObjectSchema<FormValues>)
  });

  const { handleSubmit, trigger } = methods;

  const onSubmit = handleSubmit(async (values: FormValues) => {
    console.log(values);
  });

  const handleNext = async () => {
    const isValid = await trigger(fieldsToValidate[activeStep]);
    if (isValid) {
      setActiveStep(prevActiveStep => prevActiveStep + 1);
    }
  };

  const handleBack = () => {
    setActiveStep(prevActiveStep => prevActiveStep - 1);
  };

  return (
    <FormProvider {...methods}>
      <Paper
        component="form"
        onSubmit={onSubmit}
        sx={{ maxWidth: "21cm", p: 3, mx: "auto", my: 3 }}
        elevation={4}
      >
        {/* Conditional Stepper: Mobile or Regular */}
        {!isMobile ? (
          <Stepper activeStep={activeStep}>
            {steps.map((label, i) => (
              <Step key={label}>
                <StepLabel>
                  {i === activeStep ? <strong>{label}</strong> : label}
                </StepLabel>
              </Step>
            ))}
          </Stepper>
        ) : (
          <MobileStepper
            variant="dots"
            steps={steps.length}
            position="static"
            activeStep={activeStep}
            nextButton={
              <Button size="small" onClick={handleNext} disabled={activeStep === steps.length - 1}>
                Next
              </Button>
            }
            backButton={
              <Button size="small" onClick={handleBack} disabled={activeStep === 0}>
                Back
              </Button>
            }
          />
        )}

        {/* Form Content */}
        <Stack gap={5} sx={{ mt: 5 }}>
          {activeStep === 0 && <SellerInfoForm />}
          {activeStep === 1 && <VehicleInfoForm />}
          {activeStep === 2 && <PaymentInfoForm />}

          {/* Button Controls for non-mobile Stepper */}
          {!isMobile && (
            <Stack direction="row" justifyContent="space-between">
              {activeStep > 0 && <Button onClick={handleBack}>Grįžti</Button>}
              <div />
              {activeStep < steps.length - 1 ? (
                <Button onClick={handleNext}>Tęsti</Button>
              ) : (
                <Button type="submit" variant="contained">
                  Pateikti
                </Button>
              )}
            </Stack>
          )}
        </Stack>
      </Paper>
    </FormProvider>
  );
};
