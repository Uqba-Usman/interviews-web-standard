import Vue from "vue";
import { library } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

import { faArrowLeft, faEdit } from "@fortawesome/free-solid-svg-icons";

library.add(faArrowLeft, faEdit);

Vue.component("font-awesome-icon", FontAwesomeIcon);
