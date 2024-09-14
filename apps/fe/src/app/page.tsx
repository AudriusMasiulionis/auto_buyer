"use client";

import { theme } from "@/theme";
import { ThemeProvider } from "@mui/material";
import styles from "./page.module.css";

export default function Home() {
  return (
    <ThemeProvider theme={theme}>
      <div className={styles.page}>asd</div>
    </ThemeProvider>
  );
}
