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
import { useFormContext } from "react-hook-form";
import { FormValues } from "./PPForm";

const PaymentInfoForm = () => {
  const {} = useFormContext<FormValues>();

  return (
    <Stack gap={0.5}>
      <Typography variant="h6">Atsiskaitymo informacija</Typography>
      <TextField label="Transporto priemonės kaina" />
      <FormControl>
        <FormLabel id="payment-method">Atsiskaitymo būdas:</FormLabel>
        <RadioGroup aria-labelledby="payment-method" defaultValue="">
          <FormControlLabel value="cash" control={<Radio />} label="Grynais" />
          <FormControlLabel
            value="bank_transfer"
            control={<Radio />}
            label="Bankiniu pavedimu"
          />
        </RadioGroup>
      </FormControl>
      <FormControl>
        <FormLabel id="payment-date">Atsiskaitymo momentas:</FormLabel>
        <RadioGroup aria-labelledby="payment-date" defaultValue="">
          <FormControlLabel
            value="cash"
            control={<Radio />}
            label="Sutarties sudarymo metu"
          />
          <Stack direction="row">
            <FormControlLabel
              sx={{ flex: 1 }}
              value="bank_transfer"
              control={<Radio />}
              label="Kitu metu:"
            />
            <Box sx={{ flex: 1 }}>
              <LocalizationProvider dateAdapter={AdapterDateFns}>
                <DesktopDatePicker value={new Date()} />
              </LocalizationProvider>
            </Box>
          </Stack>
        </RadioGroup>
      </FormControl>
      <TextField label="Pirkėjo elektroninis paštas" />
    </Stack>
  );
};

export default PaymentInfoForm;
