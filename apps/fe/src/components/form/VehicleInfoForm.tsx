import {
  Checkbox,
  FormControl,
  FormControlLabel,
  FormGroup,
  FormLabel,
  Radio,
  RadioGroup,
  Stack,
  TextField,
  Typography
} from "@mui/material";
import { useFormContext } from "react-hook-form";
import { FormValues } from "./PPForm";

const VehicleInfoForm = () => {
  const {} = useFormContext<FormValues>();

  return (
    <Stack gap={0.5}>
      <Typography variant="h6">Transporto priemonės informacija</Typography>
      <TextField label="SDK" />
      <TextField label="Markė (D.1) ar komercinis pavadinimas (D.3)" />
      <TextField label="Valstybinis registracijos numeris (A)" />
      <TextField label="Atpažinties (identifikavimo) numeris (E)" />
      <TextField label="Transporto priemonės registracijos liudijimo serija ir numeris" />
      <FormControl>
        <FormLabel id="technical-inspection">
          Transporto priemonės privalomoji techninė apžiūra:
        </FormLabel>
        <RadioGroup aria-labelledby="technical-inspection" defaultValue="">
          <FormControlLabel value={true} control={<Radio />} label="Galioja" />
          <FormControlLabel
            value={false}
            control={<Radio />}
            label="Negalioja"
          />
        </RadioGroup>
      </FormControl>
      <FormControl sx={{ display: "flex" }}>
        <FormLabel id="incidents">
          Transporto priemonė eismo ar kitų įvykių metu per laikotarpį, kurį
          buvau parduodamos transporto priemonės savininkas:
        </FormLabel>
        <RadioGroup aria-labelledby="incidents" defaultValue="">
          <FormControlLabel
            value={true}
            control={<Radio />}
            label="Buvo apgadinta"
          />
          <FormControlLabel
            value={false}
            control={<Radio />}
            label="Nebuuvo apgadinta"
          />
        </RadioGroup>
      </FormControl>
      <FormControl>
        <FormLabel id="known-incidents">
          Eismo ar kiti įvykiai, kuriose transporto priemonė buvo apgadinti,
          man:
        </FormLabel>
        <RadioGroup aria-labelledby="known-incidents" defaultValue="">
          <FormControlLabel value={true} control={<Radio />} label="Žinomi" />
          <FormControlLabel
            value={false}
            control={<Radio />}
            label="Nežinomi"
          />
        </RadioGroup>
      </FormControl>
      <FormGroup>
        <FormLabel>Transporto priemonės trūkumai:</FormLabel>
        <FormControlLabel control={<Checkbox />} label="Stabdžių sistemos" />
        <FormControlLabel
          control={<Checkbox />}
          label="Vairuotoju ir keleiviu saugos sistemų"
        />
        <FormControlLabel
          control={<Checkbox />}
          label="Vairo mechanizmo ir pakabos elementų"
        />
        <FormControlLabel
          control={<Checkbox />}
          label="Dujų išmetimo sistemos"
        />
        <FormControlLabel
          control={<Checkbox />}
          label="Apsvietimo ir šviesos signalizavimo įtaisų"
        />
        <FormControlLabel control={<Checkbox />} label="Kita" />
      </FormGroup>
      <TextField
        multiline
        rows={3}
        label="Informacija apie įvykius ir trūkūmus"
      />
    </Stack>
  );
};

export default VehicleInfoForm;
