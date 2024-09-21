import { Stack, TextField } from "@mui/material";
import { Controller, useFormContext } from "react-hook-form";
import { FormValues } from "./PPForm";

export const SellerInfoForm = () => {
  const { control } = useFormContext<FormValues>();

  return (
    <Stack gap={1}>
      <Controller
        control={control}
        name="personalCode"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Asmens/įmonės kodas"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="name"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Vardas Pavardė/Juridinio asmens pavadinimas"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="phone"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Telefono numeris"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="sellersEmail"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Elektroninio pašto adresas"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="sellersAddress"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Adresas"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
    </Stack>
  );
};
