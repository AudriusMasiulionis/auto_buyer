import { Stack, TextField, Typography } from "@mui/material";
import { Controller, useFormContext } from "react-hook-form";
import { FormValues } from "./PPForm";

export const SellerInfoForm = () => {
  const { control } = useFormContext<FormValues>();

  return (
    <Stack gap={1}>
      <Typography variant="h6">Pardavėjo informacija</Typography>
      <Controller
        control={control}
        name="personalCode"
        render={({ field }) => (
          <TextField {...field} label="Asmens/įmonės kodas" />
        )}
      />
      <Controller
        control={control}
        name="name"
        render={({ field }) => (
          <TextField
            {...field}
            label="Vardas Pavardė/Juridinio asmens pavadinimas"
          />
        )}
      />
      <Controller
        control={control}
        name="phone"
        render={({ field }) => (
          <TextField {...field} label="Telefono numeris" />
        )}
      />
      <Controller
        control={control}
        name="sellersEmail"
        render={({ field }) => (
          <TextField
            type="email"
            {...field}
            label="Elektroninio pašto adresas"
          />
        )}
      />
      <Controller
        control={control}
        name="sellersAddress"
        render={({ field }) => <TextField {...field} label="Adresas" />}
      />
    </Stack>
  );
};
