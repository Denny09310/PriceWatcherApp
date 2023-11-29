/* eslint-disable react-refresh/only-export-components */

import React, {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
} from "react";
import { useLocalStorageValue, useMediaQuery } from "@react-hookz/web";

interface ThemeContextState {
  theme: Theme;
  setTheme: (theme: Theme) => void;
}

const initialValues: ThemeContextState = {
  theme: "system",
  setTheme: () => null,
};

const ThemeContext = createContext(initialValues);

export const useTheme = () => {
  const context = useContext(ThemeContext);
  if (!context)
    throw new Error("'useTheme' must be used within 'ThemeProvider'");
  return context;
};

interface ThemeProviderProps {
  defaultTheme?: Theme;
  storageKey?: string;
}

export const ThemeProvider: React.FC<PropsWithChildren<ThemeProviderProps>> = ({
  children,
  defaultTheme = "system",
  storageKey = "theme-ui",
}) => {
  const { value: theme, set: setTheme } = useLocalStorageValue(storageKey, {
    defaultValue: defaultTheme,
    initializeWithValue: true,
    stringify: (data) => data,
    parse: (data, fallback) => (data ?? fallback) as Theme,
  });

  const prefersDarkScheme = useMediaQuery("(prefers-color-scheme: dark)");

  useEffect(() => {
    const toggleDarkClass =
      (theme === "system" && prefersDarkScheme) || theme === "dark";
    document.body.classList.toggle("dark", toggleDarkClass);
  }, [prefersDarkScheme, theme]);

  const value = {
    theme,
    setTheme,
  };

  return (
    <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>
  );
};

export type Theme = "system" | "light" | "dark";
