import { Stack, TextField, Typography } from "@mui/material";
import { useFormContext } from "react-hook-form";
import { FormValues } from "./PPForm";

export const SellerInfoForm = () => {
  const {} = useFormContext<FormValues>();

  return (
    <Stack gap={1}>
      <Typography variant="h6">Pardavėjo informacija</Typography>
      <TextField label="Asmens/įmonės kodas" />
      <TextField label="Vardas Pavardė/Juridinio asmens pavadinimas" />
      <TextField label="Telefono numeris" />
      <TextField label="Elektroninio pašto adresas" />
      <TextField label="Adresas" />
    </Stack>
  );
};
