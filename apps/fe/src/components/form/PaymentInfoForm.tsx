import {
  Box,
  FormControl,
  FormControlLabel,
  FormLabel,
  Radio,
  RadioGroup,
  TextField,
  Typography
} from "@mui/material";
import { Stack } from "@mui/system";
import { DesktopDatePicker, LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { useState } from "react";
import { Controller, useFormContext } from "react-hook-form";
import { FormValues } from "./PPForm";

const PaymentInfoForm = () => {
  const [isPaymentToday, setIsPaymentToday] = useState(true);

  const { control } = useFormContext<FormValues>();

  return (
    <Stack gap={1}>
      <Typography variant="h6">Atsiskaitymo informacija</Typography>
      <Controller
        name="price"
        control={control}
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Transporto priemonės kaina"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        name="paymentMethod"
        control={control}
        render={({ field, fieldState: { error } }) => (
          <FormControl>
            <FormLabel id="payment-method">Atsiskaitymo būdas:</FormLabel>
            <RadioGroup
              {...field}
              aria-labelledby="payment-method"
              defaultValue=""
            >
              <FormControlLabel
                value="cash"
                control={<Radio />}
                label="Grynais"
              />
              <FormControlLabel
                value="bank_transfer"
                control={<Radio />}
                label="Bankiniu pavedimu"
              />
            </RadioGroup>
          </FormControl>
        )}
      />

      <FormLabel id="payment-date">Atsiskaitymo momentas:</FormLabel>
      <Stack direction="row" gap={2} alignItems="end">
        <FormControl sx={{ flex: 1 }}>
          <RadioGroup
            aria-labelledby="payment-date"
            defaultValue={1}
            sx={{ flex: 1 }}
          >
            <FormControlLabel
              onChange={() => setIsPaymentToday(true)}
              value={1}
              control={<Radio />}
              label="Sutarties sudarymo metu"
            />
            <FormControlLabel
              onChange={() => setIsPaymentToday(false)}
              value={0}
              control={<Radio />}
              label="Kitu metu:"
            />
          </RadioGroup>
        </FormControl>

        <Box sx={{ flex: 1 }}>
          <Controller
            name="paymentDate"
            control={control}
            render={({ field: { value, onChange }, fieldState: { error } }) => (
              <LocalizationProvider dateAdapter={AdapterDateFns}>
                <DesktopDatePicker
                  format="yyyy/MM/dd"
                  value={value}
                  onChange={onChange}
                  disabled={isPaymentToday}
                />
              </LocalizationProvider>
            )}
          />
        </Box>
      </Stack>
      <Controller
        name="buyersEmail"
        control={control}
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Pirkėjo elektroninis paštas"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
    </Stack>
  );
};

export default PaymentInfoForm;
