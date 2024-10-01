<template>
  <div class="container top-container">
    <div class="edit-task-container">
      <div
        class="edit-task-header d-flex align-items-center justify-content-start"
      >
        <button @click="goBack" class="back-icon">
          <img src="/back-icon.png" alt="Back Icon" />
        </button>
        <h2 class="text-center flex-grow-1">Update Task</h2>
      </div>

      <div class="card edit-task-card">
        <div class="card-body">
          <form @submit.prevent="handleSave">
            <div class="form-group">
              <label for="name" class="form-label">Task Name</label>
              <input
                v-model="task.name"
                type="text"
                class="form-control"
                id="name"
                name="name"
                placeholder="Enter task name"
                required
              />
            </div>

            <div class="form-group">
              <label for="description" class="form-label"
                >Task Description</label
              >
              <textarea
                v-model="task.description"
                class="form-control"
                id="description"
                rows="3"
                name="description"
                placeholder="Enter task description"
                required
              ></textarea>
            </div>

            <button
              :disabled="isSaving"
              type="submit"
              class="btn btn-save-task mt-4"
            >
              {{ isSaving ? "Saving..." : "Save Task" }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Swal from "sweetalert2";
import { getTask, updateTask } from "~/services/taskService";

export default {
  data() {
    return {
      task: {
        name: "",
        description: "",
      },
      isSaving: false,
    };
  },
  created() {
    const id = this.$route.params.id;
    getTask(id)
      .then((response) => {
        const taskInfo = response.data;
        this.task.name = taskInfo.name;
        this.task.description = taskInfo.description;
      })
      .catch((error) => {
        Swal.fire({
          icon: "error",
          title: "An Error Occurred!",
          showConfirmButton: false,
          timer: 1500,
        });
      });
  },
  methods: {
    handleSave() {
      this.isSaving = true;
      const id = this.$route.params.id;

      updateTask(id, this.task)
        .then((response) => {
          Swal.fire({
            icon: "success",
            title: "Task updated successfully!",
            showConfirmButton: false,
            timer: 1500,
          });
          this.isSaving = false;
          this.$router.go(-1);
        })
        .catch((error) => {
          this.isSaving = false;
          Swal.fire({
            icon: "error",
            title: "An Error Occurred!",
            showConfirmButton: false,
            timer: 1500,
          });
        });
    },
    goBack() {
      this.$router.go(-1);
    },
  },
};
</script>

<style scoped>
.top-container {
  margin-top: 5rem;
}

.edit-task-header {
  display: flex;
  align-items: center;
  color: white;
  padding: 20px 40px;
  border-radius: 8px 8px 0 0;
  text-align: center;
  position: relative;
  margin-bottom: 25px;
}

.edit-task-header h2 {
  margin: 0;
  font-size: 1.5rem;
  color: #052e82;
  flex-grow: 1;
  text-align: center;
}

.back-icon {
  margin-right: 10px;
  font-size: 24px;
  color: black;
  cursor: pointer;
  transition: background-color 0.3s, border 0.3s;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #052e82;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.back-icon:hover {
  background-color: #052e82;
  color: white;
}

.back-icon img {
  width: 20px;
  height: 20px;
  filter: invert(1);
}

.edit-task-card {
  margin-top: -20px;
  border-radius: 12px;
  border: none;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  padding: 20px;
}

.form-control {
  border: none;
  border-radius: 8px;
  box-shadow: 0 12px 18px rgba(0, 0, 0, 0.05);
  padding: 12px 15px;
  margin-bottom: 20px;
}

.form-label {
  margin: 5px;
  font-weight: bold;
  color: #052e82;
}

.btn-save-task {
  background-color: #052e82;
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 8px;
  cursor: pointer;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.btn-save-task:hover {
  background-color: #3a3f6a;
}
</style>
