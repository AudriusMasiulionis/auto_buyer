"use client";

import { PPForm } from "@/components/form";
import { theme } from "@/theme";
import { Box, ThemeProvider } from "@mui/material";

export default function Home() {
  return (
    <ThemeProvider theme={theme}>
      <Box>
        <PPForm />
      </Box>
    </ThemeProvider>
  );
}
