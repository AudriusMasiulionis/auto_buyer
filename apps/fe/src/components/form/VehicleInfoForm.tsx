import { carMakes } from "@/utils";
import {
  Autocomplete,
  autocompleteClasses,
  Checkbox,
  FormControl,
  FormControlLabel,
  FormGroup,
  FormHelperText,
  FormLabel,
  Radio,
  RadioGroup,
  Stack,
  TextField
} from "@mui/material";
import { ChangeEvent, useCallback } from "react";
import { Controller, useFormContext } from "react-hook-form";
import { FormValues } from "./PPForm";

const VehicleInfoForm = () => {
  const { control, setValue, getValues } = useFormContext<FormValues>();

  const onChangeDefect = useCallback(
    (event: ChangeEvent<HTMLInputElement>, isChecked: boolean) => {
      const defects = getValues("defects");
      const value = event.currentTarget.value;

      setValue(
        "defects",
        isChecked ? [...defects, value] : [...defects.filter(v => v !== value)]
      );
    },
    [getValues, setValue]
  );

  return (
    <Stack gap={1}>
      <Controller
        control={control}
        name="sdk"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="SDK"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="make"
        render={({ field: { onChange, value }, fieldState: { error } }) => (
          <Autocomplete
            disablePortal
            freeSolo
            options={carMakes}
            value={value || ""}
            sx={{
              [`.${autocompleteClasses.endAdornment}`]: { display: "none" }
            }}
            onInputChange={(_, newValue) => onChange(newValue)}
            renderInput={params => (
              <TextField
                {...params}
                label="Markė (D.1) ar komercinis pavadinimas (D.3)"
                error={!!error}
                helperText={error?.message}
              />
            )}
          />
        )}
      />
      <Controller
        control={control}
        name="registrationNumber"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Valstybinis registracijos numeris (A)"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="mileage"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Rida (kilometrais)"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="identificationNumber"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Atpažinties (identifikavimo) numeris (E)"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="serialNumber"
        render={({ field, fieldState: { error } }) => (
          <TextField
            {...field}
            label="Transporto priemonės registracijos liudijimo serija ir numeris"
            error={!!error}
            helperText={error?.message}
          />
        )}
      />
      <Controller
        control={control}
        name="technicalInspectionIsValid"
        render={({ field, fieldState: { error } }) => (
          <FormControl>
            <FormLabel id="technical-inspection">
              Transporto priemonės privalomoji techninė apžiūra:
            </FormLabel>
            <RadioGroup
              {...field}
              aria-labelledby="technical-inspection"
              defaultValue=""
            >
              <FormControlLabel
                value="true"
                control={<Radio />}
                label="Galioja"
              />
              <FormControlLabel
                value="false"
                control={<Radio />}
                label="Negalioja"
              />
            </RadioGroup>
            {error && (
              <FormHelperText error>
                {error.message} {/* Display the error message */}
              </FormHelperText>
            )}
          </FormControl>
        )}
      />
      <Controller
        control={control}
        name="incidents"
        render={({ field, fieldState: { error } }) => (
          <FormControl sx={{ display: "flex" }}>
            <FormLabel id="incidents">
              Transporto priemonė eismo ar kitų įvykių metu per laikotarpį, kurį
              buvau parduodamos transporto priemonės savininkas:
            </FormLabel>
            <RadioGroup {...field} aria-labelledby="incidents" defaultValue="">
              <FormControlLabel
                value="true"
                control={<Radio />}
                label="Buvo apgadinta"
              />
              <FormControlLabel
                value="false"
                control={<Radio />}
                label="Nebuuvo apgadinta"
              />
            </RadioGroup>
            {error && (
              <FormHelperText error>
                {error.message} {/* Display the error message */}
              </FormHelperText>
            )}
          </FormControl>
        )}
      />
      <Controller
        control={control}
        name="knowAboutIncidents"
        render={({ field, fieldState: { error } }) => (
          <FormControl>
            <FormLabel id="known-incidents">
              Eismo ar kiti įvykiai, kuriose transporto priemonė buvo apgadinti,
              man:
            </FormLabel>
            <RadioGroup
              {...field}
              aria-labelledby="known-incidents"
              defaultValue=""
            >
              <FormControlLabel
                value="true"
                control={<Radio />}
                label="Žinomi"
              />
              <FormControlLabel
                value="false"
                control={<Radio />}
                label="Nežinomi"
              />
            </RadioGroup>
            {error && (
              <FormHelperText error>
                {error.message} {/* Display the error message */}
              </FormHelperText>
            )}
          </FormControl>
        )}
      />
      <FormGroup>
        <FormLabel>Transporto priemonės trūkumai:</FormLabel>
        <FormControlLabel
          control={
            <Checkbox value="stabdžių sistemos" onChange={onChangeDefect} />
          }
          label="Stabdžių sistemos"
        />
        <FormControlLabel
          control={
            <Checkbox
              value="vairuotoju ir keleiviu saugos sistemų"
              onChange={onChangeDefect}
            />
          }
          label="Vairuotoju ir keleiviu saugos sistemų"
        />
        <FormControlLabel
          control={
            <Checkbox
              value="vairo mechanizmo ir pakabos elementų"
              onChange={onChangeDefect}
            />
          }
          label="Vairo mechanizmo ir pakabos elementų"
        />
        <FormControlLabel
          control={
            <Checkbox
              value="dujų išmetimo sistemos"
              onChange={onChangeDefect}
            />
          }
          label="Dujų išmetimo sistemos"
        />
        <FormControlLabel
          control={
            <Checkbox
              value="apšvietimo ir šviesos signalizavimo įtaisų"
              onChange={onChangeDefect}
            />
          }
          label="Apšvietimo ir šviesos signalizavimo įtaisų"
        />
        <FormControlLabel
          control={<Checkbox value="kita" onChange={onChangeDefect} />}
          label="Kita"
        />
      </FormGroup>

      <Controller
        name="incidentsInformation"
        control={control}
        render={({ field }) => (
          <TextField
            {...field}
            multiline
            rows={3}
            label="Informacija apie įvykius ir trūkūmus"
          />
        )}
      />
    </Stack>
  );
};

export default VehicleInfoForm;
