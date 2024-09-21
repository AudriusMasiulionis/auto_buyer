"use client";

import { theme } from "@/theme";
import { ThemeProvider } from "@mui/material";

const ThemeWrapper: React.FC<{ children: React.ReactNode }> = ({
  children
}) => <ThemeProvider theme={theme}>{children}</ThemeProvider>;

export default ThemeWrapper;
