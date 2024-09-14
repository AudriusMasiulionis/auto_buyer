module.exports = {
  parser: "@typescript-eslint/parser",
  parserOptions: {
    ecmaVersion: 2020,
    sourceType: "module",
    tsconfigRootDir: __dirname,
    project: "./tsconfig.json"
  },
  extends: [
    "next/core-web-vitals",
    "next/typescript",
    "@10speed/eslint-config-ten-speed-react"
  ],
  plugins: ["@typescript-eslint"]
};
