#!/bin/bash

session="Opxel"
nvim_title=" NVIM"
build_title=" BUILD"
git_title="󰊢 GIT"
gemini_title="󰫣 GEMINI"

cd "$(dirname "$0")"

if ! tmux has-session -t "$session" 2>/dev/null; then 
    tmux new-session -d -s "$session" -n "$nvim_title"
    tmux send-keys -t "$session:$nvim_title" 'cd ../src && nvim .' C-m

    tmux new-window -t "$session:1" -n "$build_title"
    tmux send-keys -t "$session:$build_title" 'cd ../build && clear' C-m

    tmux new-window -t "$session:2" -n "$git_title"
    tmux send-keys -t "$session:$git_title" 'cd .. && clear && git status' C-m

    tmux new-window -t "$session:3" -n "$gemini_title"
    tmux send-keys -t "$session:$gemini_title" 'cd .. && clear && gemini' C-m
fi

tmux attach-session -t "$session"

