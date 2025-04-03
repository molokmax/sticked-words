<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div class="logo">Sticked Words</div>
    <div class="wrapper">
      <nav>
        <RouterLink to="/">All Cards</RouterLink>
        <!-- <RouterLink to="/">Second Button</RouterLink> -->
      </nav>
    </div>
  </header>

  <RouterView />
</template>

<style scoped lang="scss">
header {
  height: 3rem;
  display: flex;
  flex-direction: row;
  align-items: center;
  border: 1px solid var(--color-border);
}

.logo {
  display: block;
  text-align: center;
  margin: 0 2rem;
  font-weight: bold;
  font-style: italic;
}

.wrapper {
  flex: 1;
}

nav {
  text-align: center;

  a {
    display: inline-block;
    text-wrap: nowrap;
    padding: 0 1rem;
    border-left: 1px solid var(--color-border);

    &.router-link-exact-active {
      color: var(--color-text);
    }

    &.router-link-exact-active:hover {
      background-color: transparent;
    }

    &:hover {
      color: var(--color-heading);
      background-color: var(--color-background-hover);
    }

    &:first-of-type {
      border: 0;
    }
  }
}
</style>
