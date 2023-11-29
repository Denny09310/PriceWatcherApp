import ReactDOM from "react-dom/client";
import { Provider as ReduxProvider } from "react-redux";

import App from "@/App.tsx";
import { store } from "@/app/store";
import { ThemeProvider } from "@/contexts/ThemeContext";

import "@/index.css";

ReactDOM.createRoot(document.getElementById("root")!).render(
	<ReduxProvider store={store}>
		<ThemeProvider>
			<App />
		</ThemeProvider>
	</ReduxProvider>
);
