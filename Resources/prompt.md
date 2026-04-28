Prompt

<!-- TOC -->

- [Sprites](#sprites)
    - [1. Création des animations](#1-cr%C3%A9ation-des-animations)
        - [1.1. Création de Bernard](#11-cr%C3%A9ation-de-bernard)
- [Re-Pose Bernard Walk Reference](#re-pose-bernard-walk-reference)

<!-- /TOC -->

# Sprites

## Création des animations

Based on 
- https://stable-diffusion-art.com/controlnet-comfyui/#ComfyUI_ControlNet_workflows
- https://mybyways.com/blog/improving-poses-with-sdxl-controlnets

Juggernaunt:
- https://civitai.com/models/133005/juggernaut-xl?modelVersionId=240840
- https://huggingface.co/RunDiffusion/Juggernaut-X-v10
- https://civitai.com/models/133005/juggernaut-xl

Stability AI SDXL Control-LoRA depth:
- https://huggingface.co/lllyasviel/sd_control_collection/blob/main/sai_xl_depth_256lora.safetensors
- https://huggingface.co/xinsir/controlnet-depth-sdxl-1.0?utm_source=chatgpt.com (xinsir/controlnet-depth-sdxl-1.0)

OpenPose:
- https://huggingface.co/xinsir/controlnet-union-sdxl-1.0?utm_source=chatgpt.com
- https://github.com/space-nuko/ComfyUI-OpenPose-Editor
- Install ComfyUI OpenPose plugin : https://github.com/westNeighbor/ComfyUI-ultimate-openpose-editor

### Création de Bernard

# Re-Pose Bernard Walk Reference
Create a new edited PNG from `game/assets/sprites/bernard/Bernard ref.png`, using `resources/Sprites/Walking/first frame.png` as the pose reference. The edit should closely match the reference limb positions while preserving Bernard’s identity, outfit, proportions, transparent background, and painterly sprite style.

Create a sheet of 1 row and 9 frames of 209x391 images in png format with transparent background.
the first frame is [Bernard ref posed frame 1.png](game/assets/sprites/bernard/Bernard ref posed frame 1.png) 
use [sample walking.jpg](resources/Sprites/Walking/sample walking.jpg) as legs and harms pose reference.

The edit should closely match the reference limb positions while preserving Bernard’s identity, outfit, proportions, transparent background, and painterly sprite style.

Ensure coherence between frames
Ensure cohenrence in pose based on the reference.
last frame should be the same as the first frame.

you can override [bernard_walk_cycle_9.png](game/assets/sprites/bernard/bernard_walk_cycle_9.png)  as there is incoherence between frames